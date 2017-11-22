/*
* File:        Cut Scene End
* Author:      Robert Neff
* Date:        11/22/17
* Description: Loads the first level of the game after cutscene.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour {

    public void loadLevelOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // first level of game
    }
}
