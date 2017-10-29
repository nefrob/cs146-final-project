/*
* File:        Don't Destroy Music
* Author:      Robert Neff
* Date:        10/28/17
* Description: Begins music playing on a gameobject that will not
*              be destroyed on scene reload.
*/

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