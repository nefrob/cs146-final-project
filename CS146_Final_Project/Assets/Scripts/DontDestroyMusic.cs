/*
* File:        Don't Destroy Music
* Author:      Robert Neff
* Date:        11/02/17
* Description: Begins music playing on a gameobject that will not
*              be destroyed on scene reload.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMusic : MonoBehaviour {
    // Original instance
    private static DontDestroyMusic instance = null;
    public static DontDestroyMusic Instance
    {
        get { return instance; }
    }

    /* Don't destroy, if already exists destroy self.  */
    void Awake()
    {
        transform.parent = null;
        if (instance != null &&instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}