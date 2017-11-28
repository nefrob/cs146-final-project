/*
* File:        Dodge Ball
* Author:      Robert Neff
* Date:        11/09/17
* Description: Implements methods for the dodgeball object.
*              Handles collisions and provides interface to 
*              other classes.
*/

using UnityEngine;

public class DodgeBall : MonoBehaviour {
    // Parent positioning
    private Transform hand;
    [SerializeField] private bool isStartBall = false;
    // Ball state
    private CircleCollider2D myCollider;
    private Rigidbody2D rb;
    private Vector3 startLocation;
    // Ball stats
    [SerializeField] private float throwForce = 100.0f;
    // Player update
    private PlayerController playerScript;
    // Audio
    private AudioSource source;
    [SerializeField] private AudioClip bounceSound;
    // Particle system for collision flair
    [SerializeField] private ParticleSystem contactPS;
    [SerializeField] private float particlesPlayTime = -1f;

    /* Set collider. */
    void Awake()
    {
        // Loop over colliders to find and ignore
        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                myCollider = collider;  
                break;
            }
        }
    }

    // Initialize variables
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerController>();
        Physics2D.IgnoreCollision(playerScript.standingCollider, myCollider);
        source = GetComponent<AudioSource>();
        startLocation = transform.position;
        hand = GameObject.FindGameObjectWithTag("PlayerHand").transform;
        if (isStartBall)
        {
            // Disable movement
            transform.position = hand.position;
            transform.parent = hand;
            rb.simulated = false;
        }
    
        if (contactPS != null)
        {
            contactPS.Stop(); // in case starts playing
            if (particlesPlayTime == -1f) particlesPlayTime = 
                    contactPS.main.duration / contactPS.main.simulationSpeed;
        }
    }

    /* Set ignore collision again. */
    void OnEnable()
    {
        if (playerScript == null) return;
        Physics2D.IgnoreCollision(playerScript.standingCollider, myCollider);
    }

    /* Throw the ball in player facing direction. */
    public void ThrowBall(float xPlayerFacing, float powerUpForce)
    {
        DropBall(xPlayerFacing);
        float totalForce = throwForce + powerUpForce;
        rb.AddForce(new Vector2(xPlayerFacing * totalForce, 0.3f * totalForce));
    }

    /* Drops the ball from the player's hand. */
    public void DropBall(float xPlayerFacing)
    {
        transform.parent = null;
        Vector3 update = transform.position;
        update.x += xPlayerFacing * 2;
        transform.position = update;
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    /* Ball picked up by player. */
    private void playerPickMeUp()
    {
        // Pickup ball
        if (!playerScript.pickupBall) return;
        playerScript.AddBallToPlayer(this);
        playerScript.pickupBall = false;
        transform.position = hand.position;
        transform.parent = hand;
        rb.simulated = false;
        if (contactPS != null) contactPS.Stop();
    }

    /* Handle collisions with the player. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerPickMeUp();
        else if (collision.tag == "KillPlane")
        {
            transform.position = startLocation;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }

    /* Handle collisions with the player staying within ball trigger. */
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        playerPickMeUp();
    }

    /* Handle collisions with enemies and stand. */
    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Ground")
        {
            // Collide with ground sound
            if (!source.isPlaying) source.PlayOneShot(bounceSound);

            // Particle system flair
            if (contactPS == null) return;
            if (!contactPS.isPlaying)
            {
                contactPS.Play();
                Invoke("stopContactPS", particlesPlayTime);
            }
        }
    }

    /* Stops contact particle system playing. */
    private void stopContactPS()
    {
        contactPS.Stop();
    }
}
