﻿/*
* File:        Game Manager
* Author:      Robert Neff
* Date:        10/28/17
* Description: Handles the game state and scene loading.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Time to wait before starting new game
    [SerializeField] private float restartDelay = 5.0f;
    [SerializeField] private float loadDelay = 2.0f;

    // UI elements to enable/diasble upon actions
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject deathUI;

    // Game state
    private bool gameHasEnded = false;

    /* Load next level, player won. */
    public void completeLevel() {
        completeLevelUI.SetActive(true);
        Invoke("LoadNextLevel", loadDelay);
    }

    /* Restart, player lost. */
    public void endGame() {
        if (!gameHasEnded) {
            deathUI.SetActive(true);
            gameHasEnded = true;
            // Restart game
            Invoke("restart", restartDelay);
        }
    }

    /* Reload the scene. */
    void restart () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /* Switch to next scene in build order. */
    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
