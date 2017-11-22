/*
* File:        End Screen Quit
* Author:      Robert Neff
* Date:        11/21/17
* Description: Implements support to display player score and  quit the application.
*/

using UnityEngine;
using UnityEngine.UI;

public class EndScreenQuit: MonoBehaviour {
    // For player score to display
    [SerializeField] private Text scoreText;
    private DontDestroyObjects score;

    /* Set current game total score. */
    void Start()
    {
        scoreText.text = "Score: " + score.score.ToString();
    }

    /* Quit the application */
    public void Quit() {
        Application.Quit();
    }
}
