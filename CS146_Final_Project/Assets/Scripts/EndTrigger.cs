/*
* File:        End Trigger
* Author:      Robert Neff
* Date:        11/05/17
* Description: Checks whether the player has entered the end zone and
*              calls the game manager on change.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour {
    // UI
    [SerializeField] private GameObject endUI;
    [SerializeField] private GameObject ballMsg;
    // Change game state
    private GameManager gameManager;
    private PlayerController playerScript;

    /* Set tweening and get game manager. */
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerScript = FindObjectOfType<PlayerController>();
    }

    /* Trigger next level to load on reaching end zone of current. */
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player") return;

        // Make ui visible
        endUI.SetActive(true);

        // Check if found all balls, end condition
        if (playerScript.getNumBallsFound() < playerScript.numBallsToFind)
        {
            ballMsg.SetActive(true);
            return;
        } 

        // End game, all complete
        GetComponent<AudioSource>().Play();
        gameManager.completeLevel();
    }

    /* Get rid of display text. */
    void OnTriggerExit2D(Collider2D collision)
    {
        ballMsg.SetActive(false);
        endUI.SetActive(false);
    }
}
