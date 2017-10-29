/*
* File:        Pickup Controller
* Author:      Robert Neff
* Date:        10/28/17
* Description: Handles collisions with pickups, removing them as a result.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

    /* Handle pickup collision */
    void OnTriggerEnter2D(Collider2D other)
    {
		// Fill with desired effects
		// ex. play pickup scound, modfy player stats, increase score
		
        Destroy(gameObject);
    }
}
