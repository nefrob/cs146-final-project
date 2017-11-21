/*
* File:        Don't Destroy Objects
* Author:      Robert Neff
* Date:        11/02/17
* Description: Begins music playing on a gameobject that will not
*              be destroyed on scene reload. Also stores player score.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObjects : MonoBehaviour {
    // Original instance
    private static DontDestroyObjects instance = null;
    public static DontDestroyObjects Instance
    {
        get { return instance; }
    }

    // Track player score here too
    public int score;

    /* Don't destroy, if already exists destroy self.  */
    void Awake()
    {
        transform.parent = null;
        if (instance != null &&instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
            score = 0;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}