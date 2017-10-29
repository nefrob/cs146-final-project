/*
* File:        End Trigger
* Author:      Robert Neff
* Date:        10/28/17
* Description: Checks whether the player has entered the end zone and
*              calls the game manager on change.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    /* Trigger next level to load on reaching end zone of current. */
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player") return;

        //GetComponent<AudioSource>().Play();
        gameManager.completeLevel();
    }
}
