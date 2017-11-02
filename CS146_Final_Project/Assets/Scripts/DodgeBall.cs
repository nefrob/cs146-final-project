/*
* File:        Dodge Ball
* Author:      Robert Neff
* Date:        11/02/17
* Description: Implements methods for the dodgeball object.
*              Handles collisions and provides interface to 
*              other classes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBall : MonoBehaviour {
    // Parent positioning
    [SerializeField] private Transform hand;
    // Ball state
    private Rigidbody2D rb;
    // Ball stats
    private float throwForce = 15.0f;
    // Player update
    private PlayerController playerScript;
    // Audio
    private AudioSource source;
    [SerializeField] private AudioClip pickupBallSound;
    [SerializeField] private AudioClip bounceSound;

    // Initialize variables
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerController>();
        source = GetComponent<AudioSource>();
        transform.position = hand.position;
        transform.parent = hand;
        rb.simulated = false;
    }

    /* Throw the ball in player facing direction. */ 
    public void ThowBall(float xPlayerFacing)
    {
        transform.parent = null;
        Vector3 update = transform.position;
        update.x += xPlayerFacing;
        transform.position = update;
        rb.simulated = true;
        // TODO: force?
        rb.velocity = new Vector2(xPlayerFacing * throwForce, 0.3f * throwForce);
    }

    /* Drops the ball from the player's hand. */
    public void DropBall()
    {
        rb.simulated = true;
        transform.parent = null;
    }

    /* Handle collisions with enemies and player. */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Pickup ball
            if (playerScript.pickupBall) playerScript.AddBallToPlayer(this);
            playerScript.pickupBall = false;
            source.PlayOneShot(pickupBallSound);
            transform.position = hand.position;
            transform.parent = hand;
            rb.simulated = false;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            // Collide with ground
            if (!source.isPlaying) source.PlayOneShot(bounceSound);
        }
    }
}
