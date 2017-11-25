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
    // Player position for tracking
    private Transform player;

    /* Get player transform. */
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /* Update kill x position. */
    void Update()
    {
        Vector3 temp = transform.position;
        temp.x = player.position.x;
        transform.position = temp;
    }

    /* Handle collision with plane. */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die(true);
        }
    }
}
