/*
* File:        Don't Destroy Objects
* Author:      Robert Neff
* Date:        11/28/17
* Description: Begins music playing on a gameobject that will not
*              be destroyed on scene reload. Also stores player score
*              and level codes.
*/

using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjects : MonoBehaviour {
    // Original instance
    private static DontDestroyObjects instance = null;
    public static DontDestroyObjects Instance
    {
        get { return instance; }
    }

    // Track player score here too
    public int score;
    public int multiplier;

    // Level codes
    [SerializeField] private string[] codesByLevel;
    public Dictionary<string, int> levelCodes;
    public Dictionary<int, string> inverseLevelCodes;

    /* Don't destroy, if already exists destroy self.  */
    void Awake()
    {
        transform.parent = null;
        if (instance != null &&instance != this) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
            score = 0;
            multiplier = 1;

            levelCodes = new Dictionary<string, int>();
            inverseLevelCodes = new Dictionary<int, string>();
            for (int i = 0; i < codesByLevel.Length; i++)
            {
                levelCodes[codesByLevel[i]] = i + 2; // account for main menu and cutscene
                inverseLevelCodes[i + 2] = codesByLevel[i];
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}