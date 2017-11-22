/*
* File:        UI Handler
* Author:      Robert Neff
* Date:        11/21/17
* Description: Handle player UI events and display if needed.
*/

using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    // UI objects to control
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreMultiplier;
    private Color startMultColor;
    [SerializeField] private Color[] multColors;
    public Slider shieldBarSlider;
    public Slider powerUpSlider;
    [SerializeField] private Text alertText;
    [SerializeField] private Text messageText;
    [SerializeField] private Text calloutText;
    private Color calloutColor;
    public GameObject waitText;
    public GameObject reloadText;
    [SerializeField] private Text ballsText;
    [SerializeField] private Text streakText;
    [SerializeField] private string[] streakMessages;

    // Camera to shake
    public GameObject cam;

    /* Initial UI conditions. */
    void Start()
    {
        scoreMultiplier.text = "x 1";
        startMultColor = scoreMultiplier.color;
        messageText.text = "";
        alertText.text = "";
        calloutText.text = "";
        streakText.text = "";
        calloutColor = calloutText.color;
    }

    /* Sets throw power up status. */
    public void setPowerUpUI(bool shouldPowerUp, float powerUpRate)
    {
        if (shouldPowerUp)
        {
            powerUpSlider.gameObject.SetActive(true);
            if (powerUpSlider.value < 1) powerUpSlider.value += powerUpRate * Time.deltaTime;
        }
        else
        {
            powerUpSlider.gameObject.SetActive(false);
            powerUpSlider.value = 0;
        }
    }

    /* Adds value to score and updates UI component */
    public void updateScore(int score)
    {
        iTween.ScaleBy(scoreText.gameObject, iTween.Hash("x", 1.25f, "y", 1.25f, "time", 0.25f));
        iTween.ScaleBy(scoreText.gameObject, iTween.Hash("x", 0.8f, "y", 0.8f, "time", 0.25f, "delay", 0.25f));
        iTween.ShakePosition(scoreText.gameObject,
            iTween.Hash("x", Random.Range(10, 30), "y", Random.Range(10, 30), "time", 0.5f));
        scoreText.text = "Score: " + score.ToString();
    }

    /* Updates the score multiplier displayed. */
    public void updateScoreMultiplier(int mult)
    {
        if (mult == 1) scoreMultiplier.color = startMultColor;
        //else scoreMultiplier.color = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        else scoreMultiplier.color = multColors[Random.Range(0, multColors.Length)];

        iTween.ScaleBy(scoreMultiplier.gameObject, iTween.Hash("x", 2f, "y", 2f, "time", 0.25f));
        iTween.ScaleBy(scoreMultiplier.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.25f, "delay", 0.25f));
        iTween.ShakePosition(scoreMultiplier.gameObject, 
            iTween.Hash("x", Random.Range(10, 30), "y", Random.Range(10, 30), "time", 0.5f));
        scoreMultiplier.text = "x " + mult.ToString();
    }

    /* Sets the streak message. */
    public void setStreakText()
    {
        streakText.CrossFadeAlpha(1.0f, 0.00f, false);
        streakText.text = streakMessages[Random.Range(0, streakMessages.Length)];
        iTween.ScaleBy(streakText.gameObject, iTween.Hash("x", 1.6f, "y", 1.6f, "time", 0.35f));
        iTween.ScaleBy(streakText.gameObject, iTween.Hash("x", 0.625f, "y", 0.625f, "time", 0.35f, "delay", 0.35f));
        iTween.ShakePosition(streakText.gameObject,
            iTween.Hash("x", Random.Range(15, 30), "y", Random.Range(15, 30), "time", 0.7f));
        streakText.CrossFadeAlpha(0.0f, 1.0f, false);
    }

    /* Update ball collection status. */
    public void updateBallsText(int ballsFound, int ballsToFind)
    {
        ballsText.text = "Balls   " +
            ballsFound.ToString() + " : " + ballsToFind.ToString();
    }

    /* Display a message to the user. */
    public void displayMessage(string message)
    {
        messageText.text = message;
        alertText.text = "!";
        Invoke("disableAlert", 2.5f);
    }

    /* Disables the message. */
    public void hideMessage()
    {
        messageText.text = "";
    }

    /* Disables the alert text. */
    private void disableAlert()
    {
        alertText.text = "";
    } 

    /* Displays callout text in desired way on screen, 
     * ex. appear then fade out, enlarge with time, etc. */
    public void setCalloutText(string msg)
    {
        // Ben insert code here
        // You will probably have to use invoke, coroutine or do
        // something in update to implement this

        // Maybe something along the lines of:
        calloutText.CrossFadeAlpha(1.0f, 0.00f, false);
        calloutText.text = msg;
        iTween.ScaleBy(calloutText.gameObject, iTween.Hash("x", 1.6f, "y", 1.6f, "time", 0.25f));
        iTween.ScaleBy(calloutText.gameObject, iTween.Hash("x", 0.625f, "y", 0.625f, "time", 0.25f, "delay", 0.25f));
        iTween.ShakePosition(calloutText.gameObject,
            iTween.Hash("x", Random.Range(15, 30), "y", Random.Range(15, 30), "time", 0.5f));
        calloutText.CrossFadeAlpha(0.0f, 0.65f, false);
    }
}
