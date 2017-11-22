/*
* File:        Two Way Platform Collider
* Author:      Robert Neff
* Date:        10/28/17
* Description: Disables/enablles two colliders such that no collision is incurred
*              if the colliding object 'hits' the trigger colider first.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayPlatformCollider : MonoBehaviour {

    private BoxCollider2D playerCollider;
    [SerializeField] private BoxCollider2D platformCollider;
    [SerializeField] private BoxCollider2D platformTrigger;
    
    // Init vars
	void Start () {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}

    /* Ignore collision, coming from below. */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }

    /* Collide, coming from above. */
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }
}
