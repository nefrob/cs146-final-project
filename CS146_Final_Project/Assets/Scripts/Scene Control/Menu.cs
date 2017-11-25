/*
* File:        Menu
* Author:      Robert Neff
* Date:        10/28/17
* Description: Controls for menu buttons and tweening objects onto the screen.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject panel;
    [SerializeField] private float load_delay = 2.0f;

    public void Start()
    {
        //iTween.MoveBy(panel, iTween.Hash("y", -Screen.height / 2, "easeType", "easeInOutExpo"));
        //iTween.MoveBy(start, iTween.Hash("y", Screen.height / 3, "easeType", "easeInOutExpo", "delay", 1.5f));
    }

    public void StartGame() {
        Invoke("changeScene", load_delay);
        GetComponent<AudioSource>().Play();
        iTween.MoveBy(panel, iTween.Hash("y", 3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
        iTween.MoveBy(start, iTween.Hash("y", -3 * Screen.height, "easeType", "easeInOutExpo", "delay", 0.25));
    }

    public void changeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
