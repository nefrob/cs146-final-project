/*
* File:        Player Controller
* Author:      Robert Neff
* Date:        11/09/17
* Description: Implements player related systems: movement, animation, and
*              and public methods to be called upon collision with another object.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Audio
    [SerializeField] private PlayerAudio myAudio;
    //UI
    [SerializeField] private UIHandler ui;
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
    public int score = 0;
    // Animation
    [SerializeField] private Animator anim;
    // Player Systems
    [SerializeField] private GameObject forceField;
    // Balls
    [SerializeField] private DodgeBall lastBall;
    public List<DodgeBall> balls;
    private int currBall = 0;
    public int numBallsToFind = 0;
    private HashSet<DodgeBall> ballsFound;

    /* Init vars. */
    void Start () {
        myAudio = FindObjectOfType<PlayerAudio>();
        ui = FindObjectOfType<UIHandler>();
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
        ui.updateBallsText(ballsFound.Count, numBallsToFind);
    }

    /* Check for input. */
    void Update()
    {
        if (isDead) return;

        // Read the input in Update so button presses aren't missed.
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
        ui.setPowerUpUI(powerUp && hasBall && isGrounded, powerUpRate);
    }

    /* Sets player throwing status. */
    private void setThrowing()
    {
        if (hasBall && throwBall && isGrounded)
        {
            lastBall = balls[currBall];
            removeBall();
            anim.SetBool("isThrowing", true);
            myAudio.playThrowSound();
            StartCoroutine(InvokeThrow(0.28f, powerUpForce * ui.powerUpSlider.value));
        }
        else
        {
            anim.SetBool("isThrowing", false);
        }
    }

    /* Sets player shielding status. */
    private void setShielding()
    {
        if (ui.shieldBarSlider.value == 1) ui.reloadText.SetActive(false);
        if (shield && hasBall && isGrounded && ui.shieldBarSlider.value > 0 && !depletedShield)
        {
            forceField.SetActive(true);
            myAudio.shieldSource.Play();
            anim.SetBool("isShielding", true);
            ui.shieldBarSlider.value -= shieldDepleteRate * Time.deltaTime;  // reduce shield capacity
        } 
        else
        {
            if (shield && hasBall && isGrounded && ui.shieldBarSlider.value > 0 && depletedShield) ui.waitText.SetActive(true);

            myAudio.shieldSource.Stop();
            forceField.SetActive(false);
            anim.SetBool("isShielding", false);
            if (ui.shieldBarSlider.value < 1)
            {
                ui.shieldBarSlider.value += shieldRefillRate * Time.deltaTime;
                ui.reloadText.SetActive(true);
            }
            if (ui.shieldBarSlider.value <= 0.01f) depletedShield = true;
        }
        // Reset so can use again
        if (ui.shieldBarSlider.value > 0.3f)
        {
            depletedShield = false;
            ui.waitText.SetActive(false);
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
            myAudio.playerSource.PlayOneShot(myAudio.jumpSound);
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
        myAudio.playerSource.PlayOneShot(myAudio.switchBall);
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
        myAudio.playerSource.PlayOneShot(myAudio.pickupBall);
        ballsFound.Add(ball);
        ui.updateBallsText(ballsFound.Count, numBallsToFind);

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
        myAudio.playerSource.PlayOneShot(myAudio.dropSound);
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
        myAudio.playerSource.PlayOneShot(myAudio.deathSound);
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
        myAudio.playerSource.PlayOneShot(myAudio.fallingDeathSound);
        FindObjectOfType<GameManager>().endGame();
    }

    /* Invoke function to throw ball. */
    IEnumerator InvokeThrow(float delay, float force) {
        yield return new WaitForSeconds(delay);
        lastBall.ThrowBall(playerBody.transform.forward.x, force);
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
        myAudio.playerSource.PlayOneShot(myAudio.explosionSound);
    }

    /* Adds value to score and updates UI component */
    public void updateScore(int add)
    {
        score += add;
        ui.updateScore(score);
    }

    /* Reset score to zero. */
    private void resetScore()
    {
        score = 0;
        ui.updateScore(0);
    }

    /*For external scripts to note when the player has died*/
    public bool isDeadState() {
        return isDead;
    }
}
