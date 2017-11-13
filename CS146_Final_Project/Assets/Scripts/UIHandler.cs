/*
* File:        UI Handler
* Author:      Robert Neff
* Date:        11/09/17
* Description: Handle player UI events and display if needed.
*/

using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    // UI objects to control
    [SerializeField] private Text scoreText;
    public Slider shieldBarSlider;
    public Slider powerUpSlider;
    [SerializeField] private Text alertText;
    [SerializeField] private Text messageText;
    public GameObject waitText;
    public GameObject reloadText;
    [SerializeField] private Text ballsText;

    /* Initial UI conditions. */
    void Start()
    {
        messageText.text = "";
        alertText.text = "!";
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
        scoreText.text = "Score: " + score.ToString();
    }

    /* Update ball collection status. */
    public void updateBallsText(int ballsFound, int ballsToFind)
    {
        ballsText.text = "Balls: " +
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
}
