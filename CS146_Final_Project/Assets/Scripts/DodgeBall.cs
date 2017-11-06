/*
* File:        Dodge Ball
* Author:      Robert Neff
* Date:        11/05/17
* Description: Implements methods for the dodgeball object.
*              Handles collisions and provides interface to 
*              other classes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBall : MonoBehaviour {
    // Parent positioning
    private Transform hand;
    [SerializeField] private bool isStartBall = false;
    // Ball state
    private Rigidbody2D rb;
    private CircleCollider2D myCollider;
    // Ball stats
    [SerializeField] private float throwForce = 100.0f;
    // Player update
    private PlayerController playerScript;
    // Audio
    private AudioSource source;
    [SerializeField] private AudioClip[] pickupBallSound;
    [SerializeField] private AudioClip bounceSound;
    // Audio text
    [SerializeField] private GameObject niceText;
    [SerializeField] private GameObject rightousText;
    int secondsToWait = 2;

    // Initialize variables
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        playerScript = FindObjectOfType<PlayerController>();
        source = GetComponent<AudioSource>();
        hand = GameObject.FindGameObjectWithTag("PlayerHand").transform;
        if (isStartBall)
        {
            // Disable movement
            transform.position = hand.position;
            transform.parent = hand;
            rb.simulated = false;
        }
    }

    /* Throw the ball in player facing direction. */ 
    public void ThowBall(float xPlayerFacing, float powerUpForce)
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
        myCollider.enabled = true;
    }

    /* Handle collisions with enemies and player. */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Pickup ball
            if (!playerScript.pickupBall) return;
            myCollider.enabled = false;
            playerScript.AddBallToPlayer(this);
            playerScript.pickupBall = false;
            // audio 
            playBallAudio();
            // rest of picking up ball
            transform.position = hand.position;
            transform.parent = hand;
            rb.simulated = false;
        } else if (collision.gameObject.tag == "Ground")
        {
            // Collide with ground
            if (!source.isPlaying) source.PlayOneShot(bounceSound);
        } else if (collision.gameObject.tag == "Stand")
        {
            // Put on ball stand
            transform.parent = collision.gameObject.transform;
        }
    }

    void playBallAudio() {
        AudioClip nice = pickupBallSound[0];
        AudioClip rightous = pickupBallSound[1];
        AudioClip pickupBall = pickupBallSound[Random.Range(0, 2)];
        source.PlayOneShot(pickupBall);
        if (pickupBall == nice)
        {
            niceText.SetActive(true);
            Invoke("deleteText", 2);
        }
        else
        {
            rightousText.SetActive(true);
            Invoke("deleteText", 2);
        }
    }

    void deleteText() {
        niceText.SetActive(false);
        rightousText.SetActive(false);
    }

}
