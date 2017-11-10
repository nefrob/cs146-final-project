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
    public GameObject waitText;
    public GameObject reloadText;
    [SerializeField] private Text ballsText;

    /* Initial UI conditions. */
    void Start()
    {
        alertText.enabled = false;
    }

    /* Sets throw power up status. */
    public void setPowerUpUI(bool shouldPowerUp, float powerUpRate)
    {
        GameObject parent = powerUpSlider.transform.parent.gameObject;
        if (shouldPowerUp)
        {
            if (!parent.activeInHierarchy) parent.SetActive(true);
            if (powerUpSlider.value < 1) powerUpSlider.value += powerUpRate * Time.deltaTime;
        }
        else
        {
            if (parent.activeInHierarchy) parent.SetActive(false);
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
            ballsFound.ToString() + " / " + ballsToFind.ToString();
    }
}
