using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMusic : MonoBehaviour {

    static bool AudioBegin = false;

    /* Play music on wake (menu), don't destroy on next scene load */
    void Awake() {
        if (!AudioBegin) {
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }
}