/*
* File:        Kill Plane
* Author:      Robert Neff
* Date:        10/28/17
* Description: Checks whether the player has entered the kill plane; if so
*              triggers player to die.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour {

    /* Handle collision with plane. */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
