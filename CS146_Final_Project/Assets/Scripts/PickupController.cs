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
