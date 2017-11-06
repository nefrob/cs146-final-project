/*
* File:        Player Controller
* Author:      Robert Neff
* Date:        11/05/17
* Description: Implements player related systems: movement, animation, and
*              and public methods to be called upon collision with another object.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // Movement parameters
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float shieldDepleteRate = 0.5f;
    [SerializeField] private float shieldRefillRate = 0.1f;
    [SerializeField] private float powerUpRate = 0.1f;
    [SerializeField] private float powerUpForce = 50f;
    [SerializeField] private GameObject playerBody;
    private Rigidbody2D rb;
    // Ground status
    [SerializeField] private Transform[] groundPoints;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool airControl;
    // Audio options
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioSource shieldSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip[] throwSounds;
    [SerializeField] private AudioClip shieldSound; // should be able to loop
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip fallingDeathSound;
    [SerializeField] private AudioClip switchBall;
    // UI
    [SerializeField] private Text scoreText;
    public Slider shieldBarSlider;
    [SerializeField] private Slider powerUpSlider;
    [SerializeField] private Text alertText;
    [SerializeField] private GameObject waitText;
    [SerializeField] private GameObject reloadText;
    [SerializeField] private Text ballsText;
    public int score = 0;
    // Player status
    private bool facingRight;
    private bool hasBall;
    public  bool pickupBall;
    private bool isGrounded;
    private bool jump;
    private bool throwBall;
    private bool dropBall;
    private bool shield;
    private bool isDead;
    private bool depletedShield;
    private bool powerUp;
    // Animation
    [SerializeField] private Animator anim;
    // Player Systems
    [SerializeField] private GameObject forceField;
    [SerializeField] private DodgeBall lastBall;
    // Balls
    public List<DodgeBall> balls;
    private int currBall = 0;
    public int numBallsToFind = 0;
    private HashSet<DodgeBall> ballsFound;

    /* Init vars. */
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        hasBall = true;
        pickupBall = false;
        throwBall = false;
        dropBall = false;
        isDead = false;
        depletedShield = false;
        powerUp = false;
        balls = new List<DodgeBall>();
        ballsFound = new HashSet<DodgeBall>();
        ballsFound.Add(lastBall);
        balls.Add(lastBall);
        updateBallsText();
        playerSource.clip = jumpSound;
        alertText.enabled = false;
    }

    /* Check for input. */
    void Update()
    {
        if (isDead) return;

        // Read the jump input in Update so button presses aren't missed.
        if (!jump) jump = Input.GetButtonDown("Jump");
        throwBall = Input.GetKeyUp(KeyCode.Q);// Input.GetButtonDown("Fire1");
        powerUp = Input.GetKey(KeyCode.Q);
        shield = Input.GetButton("Fire2");
        setCurrBallMouse();
        pickupBall = Input.GetKey(KeyCode.LeftShift);
        dropBall = Input.GetKeyDown(KeyCode.E);
    }

    /* Compute physics and movement. */
    void FixedUpdate () {
        if (isDead) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isGrounded = getIsGrounded();
        HandlePlayerSystems(horizontal, vertical);
        Flip(horizontal);
        jump = false; // reset input
    }

    /* Handle all forms of player movement. */
    private void HandlePlayerSystems(float horizontal, float vertical)
    {
        // Set the vertical animation
        anim.SetFloat("vSpeed", rb.velocity.y);
        // Set grounded animation
        anim.SetBool("isGrounded", isGrounded);

        // Update player state
        if (dropBall) playerDropBall();
        setThrowing();
        setPowerUp();
        setShielding();
        setMovement(horizontal);
        setJumping();
    }

    /* Sets layer power up status. */
    private void setPowerUp()
    {
        GameObject parent = powerUpSlider.transform.parent.gameObject;
        if (powerUp && hasBall && isGrounded)
        {
            if (!parent.activeInHierarchy) parent.SetActive(true);
            if (powerUpSlider.value < 1) powerUpSlider.value += powerUpRate * Time.deltaTime;
        }
        else
        {
            if (parent.activeInHierarchy) parent.SetActive(false);
            powerUpSlider.value = 0;
        }
    }

    /* Sets player throwing status. */
    private void setThrowing()
    {
        if (hasBall && throwBall && isGrounded)
        {
            lastBall = balls[currBall];
            removeBall();
            anim.SetBool("isThrowing", true);
            playerSource.clip = throwSounds[Random.Range(0, 3)];
            playerSource.Play();
            StartCoroutine(InvokeThrow(0.28f, powerUpForce * powerUpSlider.value));
        }
        else
        {
            anim.SetBool("isThrowing", false);
        }
    }

    /* Sets player shielding status. */
    private void setShielding()
    {
        if (shieldBarSlider.value == 1) reloadText.SetActive(false);
        if (shield && hasBall && isGrounded && shieldBarSlider.value > 0 && !depletedShield)
        {
            forceField.SetActive(true);
            shieldSource.Play();
            anim.SetBool("isShielding", true);
            shieldBarSlider.value -= shieldDepleteRate * Time.deltaTime;  // reduce shield capacity
        } 
        else
        {
            if (shield && hasBall && isGrounded && shieldBarSlider.value > 0 && depletedShield) waitText.SetActive(true);

            forceField.SetActive(false);
            shieldSource.Stop();
            anim.SetBool("isShielding", false);
            if (shieldBarSlider.value < 1)
            {
                shieldBarSlider.value += shieldRefillRate * Time.deltaTime;
                reloadText.SetActive(true);
            }
            if (shieldBarSlider.value <= 0.01f) depletedShield = true;
        }
        // Reset so can use again
        if (shieldBarSlider.value > 0.3f)
        {
            depletedShield = false;
            waitText.SetActive(false);
        }
    }

    /* Sets player movement status. */
    private void setMovement(float horizontal)
    {
        // Disbale movement if shielding or throwing
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shield") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Throw")) horizontal = 0.0f;

        // Set movement
        if (isGrounded || airControl) rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        anim.SetFloat("hSpeed", Mathf.Abs(horizontal));
    }

    /* Sets player jumping status. */
    private void setJumping()
    {
        if (isGrounded && jump && anim.GetBool("isGrounded"))
        {
            playerSource.PlayOneShot(jumpSound);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
        }
    }

    /* Sets current ball from scroll wheel. */
    private void setCurrBallMouse()
    {
        // No balls return
        if (balls.Count <= 1) return;

        // Change on input
        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input == 0) return;

        balls[currBall].gameObject.SetActive(false);
        playerSource.PlayOneShot(switchBall);
        if (input > 0f)
        {
            // scroll up
            currBall += 1;
            if (currBall >= balls.Count) currBall = 0;
        }
        else if (input < 0f)
        {
            // scroll down
            currBall -= 1;
            if (currBall < 0) currBall = balls.Count - 1;
        }
        balls[currBall].gameObject.SetActive(true);
    }

    /* Pickup dodgeball. */
    public void AddBallToPlayer(DodgeBall ball)
    {
        // Set that has a ball
        hasBall = true;
        ballsFound.Add(ball);
        updateBallsText();

        if (balls.Count == 0)
        {
            // Add first ball
            balls.Add(ball);
            currBall = 0;
        }
        else
        {
            // Change to new ball
            balls[currBall].gameObject.SetActive(false);
            balls.Add(ball);
            currBall = balls.Count - 1;
        }
    }

    /* Number of unique balls found. */
    public int getNumBallsFound()
    {
        return ballsFound.Count;
    }

    /* Drops current ball on the ground. */
    private void playerDropBall()
    {
        if (balls.Count == 0) return;
        playerSource.PlayOneShot(dropSound);
        lastBall = balls[currBall];
        removeBall();
        lastBall.DropBall(playerBody.transform.forward.x);
    }

    /* Flips player facing direction */
    private void Flip(float horizontal)
    {
        // Set movement direction
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    /* Checks whether the player is grounded */
    private bool getIsGrounded()
    {
        if (rb.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject && colliders[i].tag != "Climbable")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /* Perform kill action. */
    public void Die()
    {
        if (isDead) return;

        // Reset all values
        anim.SetBool("isDead", true);
        anim.SetBool("isGrounded", true);
        isDead = true;
        resetScore();
        playerSource.PlayOneShot(deathSound);
        FindObjectOfType<GameManager>().endGame();
    }

    /* Perform kill action if falling. */
    public void FallDie()
    {
        if (isDead) return;

        // Reset all values
        anim.SetBool("isFallingDead", true);
        anim.SetBool("isGrounded", true);
        iTween.RotateBy(playerBody, iTween.Hash("y", 0.9, "easeType", "easeInOutBack", "time", 5.0f));
        isDead = true;
        resetScore();
        playerSource.PlayOneShot(fallingDeathSound);
        FindObjectOfType<GameManager>().endGame();
    }

    /* Invoke function to throw ball. */
    IEnumerator InvokeThrow(float delay, float force) {
        yield return new WaitForSeconds(delay);
        lastBall.ThowBall(playerBody.transform.forward.x, force);
    }

    /* Updates balls list and character to reflect current ball. */
    private void removeBall()
    {
        balls.Remove(lastBall);
         // Set next ball as active ball
         if (balls.Count > 0)
         {
            currBall--;
            if (currBall < 0) currBall = balls.Count - 1;
            balls[currBall].gameObject.SetActive(true);
        } else
         {
             hasBall = false;
         }
    }

    /* Play explosion sound. */ 
    public void playExplosionSound()
    {
        playerSource.PlayOneShot(explosionSound);
    }

    /* Update ball collection status. */
    private void updateBallsText()
    {
        ballsText.text = "Balls: " + 
            ballsFound.Count.ToString() + " / " + numBallsToFind.ToString();
    }

    /* Adds value to score and updates UI component */
    public void updateScore(int add)
    {
        score += add;
        scoreText.text = "Score: " + score.ToString();
    }

    /* Reset score to zero. */
    private void resetScore()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }
}
