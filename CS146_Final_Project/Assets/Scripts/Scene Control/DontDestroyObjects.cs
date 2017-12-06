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
    public static DontDestroyObjects instance = null;

    // Track player score here too
    public static int score;
    public static int multiplier;

    // Level codes
    [SerializeField] private string[] codesByLevel;
    public static Dictionary<string, int> levelCodes;
    public static Dictionary<int, string> inverseLevelCodes;

    /* Don't destroy, if already exists destroy self.  */
    void Awake()
    {
        transform.parent = null;
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        if (instance == null) { 
            instance = this;
            score = 0;
            multiplier = 1;

            levelCodes = new Dictionary<string, int>();
            inverseLevelCodes = new Dictionary<int, string>();
            for (int i = 0; i < codesByLevel.Length; i++)
            {
                levelCodes[codesByLevel[i].ToLower()] = i + 2; // account for main menu and cutscene
                inverseLevelCodes[i + 2] = codesByLevel[i];
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}