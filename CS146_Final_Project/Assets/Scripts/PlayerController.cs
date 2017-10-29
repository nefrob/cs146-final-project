/*
* File:        Player Controller
* Author:      Robert Neff
* Date:        10/28/17
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
    [SerializeField] private Transform body;
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
    [SerializeField] private AudioClip shieldSound;
    // UI
    [SerializeField] private Text scoreText;
    public int score = 0;
    // Player status
    private bool facingRight;
    private bool hasBall;
    private bool isGrounded;
    private bool jump;
    private bool throwBall;
    private bool shield;
    private bool isDead;
    // Animation
    [SerializeField] private Animator anim;
    // Player Systems
    [SerializeField] private GameObject forceField;
    private DodgeBall dodgeBallScript;

    /* Init vars. */
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        hasBall = true;
        throwBall = false;
        isDead = false;
        forceField.SetActive(false);
        dodgeBallScript = FindObjectOfType<DodgeBall>();
    }

    /* Check for input. */
    void Update()
    {
        if (isDead) return;

        // Read the jump input in Update so button presses aren't missed.
        if (!jump) jump = Input.GetButtonDown("Jump");
        if (!throwBall) throwBall = Input.GetButtonDown("Fire1");
        if (!shield) shield = Input.GetButton("Fire2");
    }

    /* Compute physics and movement. */
    void FixedUpdate () {
        if (isDead) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isGrounded = getIsGrounded();
        HandleMovement(horizontal, vertical);
        Flip(horizontal);
        jump = false; // reset input
        throwBall = false;
        shield = false;
    }

    /* Handle all forms of player movement. */
    private void HandleMovement(float horizontal, float vertical)
    {
        // Set the vertical animation
        anim.SetFloat("vSpeed", rb.velocity.y);

        // Set grounded animation
        anim.SetBool("isGrounded", isGrounded);

        // Set throwing
        if (hasBall && throwBall && isGrounded)
        {
            anim.SetBool("isThrowing", true);
			source.PlayOneShot(throwSound);
            hasBall = false;
            Invoke("InvokeThrow", 0.75f);
        }
        else
        {
            anim.SetBool("isThrowing", false);
        }

        // Set shielding - TODO: put on cooldown, add collider?
        if (shield && hasBall && isGrounded)
        {
            forceField.SetActive(true);
            source.PlayOneShot(shieldSound);
            anim.SetBool("isShielding", true);
        }
        else
        {
            forceField.SetActive(false);
            anim.SetBool("isShielding", false);
        }

        // Disbale movement if shielding
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Shield") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Throw")) horizontal = 0.0f;

        // Set movement
        if (isGrounded || airControl) rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        anim.SetFloat("hSpeed", Mathf.Abs(horizontal));

        // Set jumping
        if (isGrounded && jump && anim.GetBool("isGrounded"))
        {
            source.PlayOneShot(jumpSound);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
        }
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
        isDead = true;
        FindObjectOfType<GameManager>().endGame();
    }

    /* Invoke function to throw ball. */
    private void InvokeThrow() {
        dodgeBallScript.ThowBall(body.forward.x);
    }

    /* Pickup dodgeball. */
    public void pickupBall()
    {
        hasBall = true;
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
        scoreText.text = score.ToString();
    }
}
