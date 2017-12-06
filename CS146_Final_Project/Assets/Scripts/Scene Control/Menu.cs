/*
* File:        Menu
* Author:      Robert Neff
* Date:        10/28/17
* Description: Controls for menu buttons and tweening objects onto the screen.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject creditsBtn;
    [SerializeField] private GameObject codeBtn;
    [SerializeField] private GameObject submitBtn;
    [SerializeField] private GameObject codeExitBtn;
    [SerializeField] private GameObject codeField;
    [SerializeField] private Text codeFieldText;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject infoBtn;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private float load_delay = 2.0f;

    private bool infoDisplayed = false;

    /* Tween objects onto screen. */
    public void Start()
    {        
        iTween.MoveBy(startBtn, iTween.Hash("y", Screen.height / 3, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(creditsBtn, iTween.Hash("y", Screen.height / 3, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(codeBtn, iTween.Hash("y", Screen.height / 3, "easeType", "easeInOutExpo", "delay", 1f));
    }

    /* Check for exit input. */
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    /* Start game after delay. */
    public void StartGame() {
        Invoke("changeScene", load_delay);
        GetComponent<AudioSource>().Play();
        iTween.MoveBy(panel, iTween.Hash("y", 3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
        iTween.MoveBy(startBtn, iTween.Hash("y", -3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
        iTween.MoveBy(creditsBtn, iTween.Hash("y", -3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
        iTween.MoveBy(codeBtn, iTween.Hash("y", -3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
    }

    /* Change to next scene. */
    public void changeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /* Load credits scene. */
    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    /* Open input field for codes. */
    public void loadCodesField()
    {
        iTween.MoveBy(startBtn, iTween.Hash("y", -Screen.height / 3, "easeType", "easeInOutExpo", "delay", 0.5f));
        iTween.MoveBy(creditsBtn, iTween.Hash("y", -Screen.height / 3, "easeType", "easeInOutExpo", "delay", 0.5f));
        iTween.MoveBy(codeBtn, iTween.Hash("y", -Screen.height / 3, "easeType", "easeInOutExpo", "delay", 0.5f));

        iTween.MoveBy(codeField, iTween.Hash("y", -2.35 * Screen.height / 3, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(submitBtn, iTween.Hash("x", Screen.width / 2.25, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(codeExitBtn, iTween.Hash("x", -Screen.width / 2.25, "easeType", "easeInOutExpo", "delay", 1f));
    }

    /* Submit code from code field. */
    public void submitCode()
    {
        string entered = codeFieldText.text;
        if (DontDestroyObjects.levelCodes.ContainsKey(entered.ToLower()))
            SceneManager.LoadScene(DontDestroyObjects.levelCodes[entered.ToLower()]);
        else
        {
            iTween.ScaleBy(codeField, iTween.Hash("x", 1.2f, "y", 1.2f, "time", 0.25f));
            iTween.ScaleTo(codeField, iTween.Hash("x", 2, "y", 2, "time", 0.25f, "delay", 0.25f));
            iTween.ShakePosition(codeField,
                iTween.Hash("x", Random.Range(10, 30), "y", Random.Range(10, 30), "time", 0.5f));
        }
    }

    /* Exit codes field screen. */
    public void exitCodesField()
    {
        iTween.MoveBy(startBtn, iTween.Hash("y", Screen.height / 3f, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(creditsBtn, iTween.Hash("y", Screen.height / 3f, "easeType", "easeInOutExpo", "delay", 1f));
        iTween.MoveBy(codeBtn, iTween.Hash("y", Screen.height / 3f, "easeType", "easeInOutExpo", "delay", 1f));

        iTween.MoveBy(codeField, iTween.Hash("y", 2.35f * Screen.height / 3f, "easeType", "easeInOutExpo", "delay", 0.5f));
        iTween.MoveBy(submitBtn, iTween.Hash("x", -Screen.width / 2.25f, "easeType", "easeInOutExpo", "delay", 0.5f));
        iTween.MoveBy(codeExitBtn, iTween.Hash("x", Screen.width / 2.25f, "easeType", "easeInOutExpo", "delay", 0.5f));
    }

    /* Open info screen. */
    public void openInfo()
    {
        if (infoDisplayed) return;
        infoDisplayed = true;
        iTween.MoveBy(infoPanel, iTween.Hash("y", -Screen.height / 1.1f, "easeType", "easeInOutExpo", "delay", 0.5f));
    }

    /* Close info screen. */
    public void closeInfo()
    {
        infoDisplayed = false;
        iTween.MoveBy(infoPanel, iTween.Hash("y", Screen.height / 1.1f, "easeType", "easeInOutExpo", "delay", 0.5f));
    }
}
