using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Time to wait before starting new game
    [SerializeField] private float restartDelay = 2.0f;
    [SerializeField] private float loadDelay = 2.0f;

    // UI elements to enable/diasble upon actions
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private GameObject symbol;

    // Game state
    private bool gameHasEnded = false;

    /* Load next level, player won. */
    public void completeLevel() {
        completeLevelUI.SetActive(true);
        symbol.SetActive(false);
        Invoke("LoadNextLevel", loadDelay);
    }

    /* Restart, player lost. */
    public void endGame() {
        if (!gameHasEnded) {
            gameHasEnded = true;
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
}
