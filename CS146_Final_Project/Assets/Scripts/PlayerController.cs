/*
* File:        Player Controller
* Author:      Robert Neff
* Date:        11/02/17
* Description: Implements player reltaed systems: movement, animation, and
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
    [SerializeField] private GameObject playerBody;
    private Rigidbody2D rb;
    // Ground status
    [SerializeField] private Transform[] groundPoints;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool airControl;
    // Audio options
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip shieldSound; // should be able to loop
    [SerializeField] private AudioClip deathSound;
    // UI
    [SerializeField] private Text scoreText;
    [SerializeField] private Slider shieldBarSlider;
    public int score = 0;
    // Player status
    private bool facingRight;
    private bool hasBall;
    public  bool pickupBall = false;
    private bool isGrounded;
    private bool jump;
    private bool throwBall;
    private bool shield;
    private bool isDead;
    private bool depletedShield = false;
    // Animation
    [SerializeField] private Animator anim;
    // Player Systems
    [SerializeField] private GameObject forceField;
    private DodgeBall dodgeBallScript;
    // Balls
    public List<DodgeBall> balls = new List<DodgeBall>();
    private int currBall = -1;

    /* Init vars. */
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        hasBall = true;
        throwBall = false;
        isDead = false;
        forceField.SetActive(false);
        dodgeBallScript = FindObjectOfType<DodgeBall>();
        source.clip = shieldSound;
        source.loop = true;
    }

    /* Check for input. */
    void Update()
    {
        if (isDead) return;

        // Read the jump input in Update so button presses aren't missed.
        if (!jump) jump = Input.GetButtonDown("Jump");
        if (!throwBall) throwBall = Input.GetButtonDown("Fire1");
        shield = Input.GetButton("Fire2");
        //setCurrBallMouse();
        //pickupBall = Input.GetKeyDown(KeyCode.LeftShift);
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
        throwBall = false;
    }

    /* Handle all forms of player movement. */
    private void HandlePlayerSystems(float horizontal, float vertical)
    {
        // Set the vertical animation
        anim.SetFloat("vSpeed", rb.velocity.y);
        // Set grounded animation
        anim.SetBool("isGrounded", isGrounded);

        // Update player state
        setThrowing();
        setShielding();
        setMovement(horizontal);
        setJumping();
    }

    /* Sets player throwing status. */
    private void setThrowing()
    {
        if (hasBall && throwBall && isGrounded)
        {
            anim.SetBool("isThrowing", true);
            source.PlayOneShot(throwSound);
            hasBall = false;
            // TODO: set current ball
            Invoke("InvokeThrow", 0.25f);
        }
        else
        {
            anim.SetBool("isThrowing", false);
        }
    }

    /* Sets player shielding status. */
    private void setShielding()
    {
        if (shield && hasBall && isGrounded && shieldBarSlider.value > 0 && !depletedShield)
        {
            forceField.SetActive(true);
            source.Play();
            anim.SetBool("isShielding", true);
            shieldBarSlider.value -= shieldDepleteRate * Time.deltaTime;  // reduce shield capacity
        }
        else
        {
            forceField.SetActive(false);
            source.Stop();
            anim.SetBool("isShielding", false);
            if (shieldBarSlider.value < 1) shieldBarSlider.value += shieldRefillRate * Time.deltaTime;
            if (shieldBarSlider.value <= 0.01f) depletedShield = true;
        }
        // Reset so can use again
        if (shieldBarSlider.value > 0.3f) depletedShield = false;
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
            source.PlayOneShot(jumpSound);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
        }
    }

    /* Sets current ball from scroll wheel. */
    private void setCurrBallMouse()
    {
        // No balls return
        if (balls.Count == 0) return;

        // Change on input
        float input = Input.GetAxis("Mouse ScrollWheel");
        balls[currBall].gameObject.SetActive(false);

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

    /* Drops current ball on the ground. */
    private void dropBall()
    {
        if (balls.Count == 0) return;
        balls[currBall].DropBall();

        // TODO: Set current ball
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
        anim.SetBool("isDead", true);
        anim.SetBool("isGrounded", true);
        isDead = true;
        source.PlayOneShot(deathSound);
        FindObjectOfType<GameManager>().endGame();
    }

    /* Perform kill action if falling. */
    public void FallDie()
    {
        anim.SetBool("isFallingDead", true);
        anim.SetBool("isGrounded", true);
        iTween.RotateBy(playerBody, iTween.Hash("y", 0.9, "easeType", "easeInOutBack", "time", 5.0f));
        isDead = true;
        source.PlayOneShot(deathSound);
        FindObjectOfType<GameManager>().endGame();
    }

    /* Invoke function to throw ball. */
    private void InvokeThrow() {
        // TODO: set current ball
        dodgeBallScript.ThowBall(playerBody.transform.forward.x);
    }

    /* Play pickup sound and updates score */ 
    public void playPickupSound()
    {
        updateScore(1);
        source.PlayOneShot(pickupSound);
    }

    /* Adds value to score and updates UI component */
    public void updateScore(int add)
    {
        score += add;
        scoreText.text = "Score: " + score.ToString();
    }
}
