/*
* File:        Cutscene Logic
* Author:      Robert Neff
* Date:        12/01/17
* Description: Displays button and allows for scene switching after cutscene.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneLogic : MonoBehaviour {
    // Button to continue scene
    [SerializeField] private Image continueImg;
    [SerializeField] private Text continueText;
    [SerializeField] private float fadeSpeed = 0.5f;
    // For checking if using mashing keys to switch
    bool canSwitch = false;
    [SerializeField] private float displayBeforeFade = 2.0f;
    float timer = 0.0f;
    // Animation status
    bool animBoolDone = false;
    [SerializeField] private float animationLength = 67.0f;

    /* Hide button initially. */
    void Start () {
        continueImg.CrossFadeAlpha(0.0f, 0.0f, false);
        continueText.CrossFadeAlpha(0.0f, 0.0f, false);

        Invoke("animDone", animationLength);
    }
	
	/* Check if user wants to switch. */
	void Update () {


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (canSwitch)
            {
                Continue();
            } else
            {
                continueImg.CrossFadeAlpha(1.0f, fadeSpeed, false);
                continueText.CrossFadeAlpha(1.0f, fadeSpeed, false);
                timer = 0.0f;
                canSwitch = true; // set so if hit buttons again, scene will switch
            }
        }

        // Set display time before fade
        if (timer < displayBeforeFade)
        {
            timer += Time.deltaTime;

        } else
        {
            if (continueImg.color.a == 0.0f || animBoolDone) return;
            continueImg.CrossFadeAlpha(0.0f, fadeSpeed, false);
            continueText.CrossFadeAlpha(0.0f, fadeSpeed, false);
            canSwitch = false;
        }
    }

    /* Updates the animation status. */
    void animDone()
    {
        animBoolDone = true;
        continueText.text = "Continue";
        continueImg.CrossFadeAlpha(1.0f, fadeSpeed, false);
        continueText.CrossFadeAlpha(1.0f, fadeSpeed, false);
        canSwitch = true;
        timer = 0.0f;
    }

    /* Moves to next scene of the game. */
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // first level of game
    }
}
