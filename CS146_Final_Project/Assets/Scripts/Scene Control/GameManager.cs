/*
* File:        Game Manager
* Author:      Robert Neff
* Date:        12/02/17
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
    [SerializeField] private GameObject quitUI;
    private bool quitUIDisplayed = false;

    // Game state
    private bool gameHasEnded = false;

    /* Handle input. */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) displayQuitUI();
    }

    /* Load next level, player won. */
    public void completeLevel() {
        completeLevelUI.SetActive(true);
        gameHasEnded = true;
        quitUI.SetActive(false);
        Time.timeScale = 1.0f;
        Invoke("LoadNextLevel", loadDelay);
    }

    /* Restart, player lost. */
    public void endGame() {
        if (!gameHasEnded) {
            deathUI.SetActive(true);
            gameHasEnded = true;
            quitUI.SetActive(false);
            Time.timeScale = 1.0f;
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

    /* Pauses the game and gives a quit option (to menu or out of application). */
    private void displayQuitUI()
    {
        if (gameHasEnded) return;
        if (quitUI.activeInHierarchy)
        {
            quitUI.SetActive(false);
            Time.timeScale = 1.0f; // play
        }
        else
        {
            quitUI.SetActive(true);
            Time.timeScale = 0.0f; // pause
        }
    } 

    /* UI quit game. */
    public void quitGame()
    {
        Application.Quit();
    }

    /* Quit to menu. */
    public void quitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f; // play
    }
}
