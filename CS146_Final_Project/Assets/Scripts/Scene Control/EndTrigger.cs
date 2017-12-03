/*
* File:        End Trigger
* Author:      Robert Neff
* Date:        12/02/17
* Description: Checks whether the player has entered the end zone and
*              calls the game manager on change.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour {
    // UI
    [SerializeField] private GameObject ballMsg;
    [SerializeField] private Text msg;
    // Change game state
    private GameManager gameManager;
    private PlayerController playerScript;
    [SerializeField] private float delay = 3.0f;
    private DontDestroyObjects endCodes;
    // End position
    [SerializeField] private Transform endPos;
    private PlayerAudio audioScript;

    /* Set tweening and get game manager. */
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerScript = FindObjectOfType<PlayerController>();
        endCodes = FindObjectOfType<DontDestroyObjects>();
        audioScript = FindObjectOfType<PlayerAudio>();
    }

    /* Trigger next level to load on reaching end zone of current. */
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player") return;

        // Check if found all balls, end condition
        if (playerScript.getNumBallsFound() < playerScript.numBallsToFind)
        {
            ballMsg.SetActive(true);
            return;
        } 

        // End game, all complete
        GetComponent<AudioSource>().Play();
        playerScript.gameObject.GetComponent<Rigidbody2D>().simulated = false; // disable movement
        playerScript.setUnkillableIdle(endPos.position);
        audioScript.playCelebrationSound();
        msg.text = "Level Code: " + endCodes.inverseLevelCodes[SceneManager.GetActiveScene().buildIndex];

        Invoke("loadNext", delay);
    }

    /* Delay time to load next level. */
    private void loadNext()
    {
        gameManager.completeLevel();
    }

    /* Get rid of display text. */
    void OnTriggerExit2D(Collider2D collision)
    {
        ballMsg.SetActive(false);
    }
}
