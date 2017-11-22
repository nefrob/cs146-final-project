/*
* File:        End Screen
* Author:      Robert Neff
* Date:        11/22/17
* Description: Implements support to display player score, credits and  quit the application.
*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndScreen: MonoBehaviour {
    // For player score to display
    [SerializeField] private Text scoreText;
    private DontDestroyObjects score;
    // For scrolling credits
    [SerializeField] private GameObject scrollObj;
    // Buttons to fade
    [SerializeField] private Image quitImg;
    [SerializeField] private Text quitText;
    [SerializeField] private Image scrollImg;
    [SerializeField] private Text scrollText;
    [SerializeField] private float fadeSpeed = 1.0f;

    /* Set current game total score anmd credits to scroll. */
    void Start()
    {
       // score = GetComponent<DontDestroyObjects>();
       // scoreText.text = "Score: " + score.score.ToString();
        iTween.MoveBy(scrollObj, iTween.Hash("y", 22000, 
            "easeType", "linear", "delay", 5.0f, "time", 90.0f));
        Invoke("exit", 5.0f);
    }

    /* Quit the application. */
    public void Quit() {
        Application.Quit();
    }

    /* Load first scene and reset values to replay game. */
    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // first level of game
        score.score = 0; 
        score.multiplier = 1;
    }

    /* Handle hover over a button. */
    public void enter()
    {
        Debug.Log("enter");
        quitImg.CrossFadeAlpha(1.0f, fadeSpeed, false);
        quitText.CrossFadeAlpha(1.0f, fadeSpeed, false);
        scrollImg.CrossFadeAlpha(1.0f, fadeSpeed, false);
        scrollText.CrossFadeAlpha(1.0f, fadeSpeed, false);
    }

    /* Handle exit of hover over a button. */
    public void exit()
    {
        quitImg.CrossFadeAlpha(0.0f, fadeSpeed, false);
        quitText.CrossFadeAlpha(0.0f, fadeSpeed, false);
        scrollImg.CrossFadeAlpha(0.0f, fadeSpeed, false);
        scrollText.CrossFadeAlpha(0.0f, fadeSpeed, false);
    }
}
