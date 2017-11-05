﻿/*
* File:        Moving Platform
* Author:      Robert Neff
* Date:        11/06/17
* Description: Makes object move back and forth continously on the specified axis
*              for given time and direction. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    // iTween movement vars
    [SerializeField] private bool useX = false;
    [SerializeField] private bool useY = false;
    [SerializeField] private float time = 1.0f;
    [SerializeField] private float distance = 10.0f;

    // Start looped movement
    void Start () {
        if (useX) iTween.MoveBy(gameObject, iTween.Hash("x", distance, 
            "easeType", "easeInOutExpo", "time", time, "looptype", "pingpong"));
        else if (useY) iTween.MoveBy(gameObject, iTween.Hash("y", distance, 
            "easeType", "easeInOutExpo", "time", time, "looptype", "pingpong"));
        else iTween.MoveBy(gameObject, iTween.Hash("z", distance, 
            "easeType", "easeInOutExpo", "time", time, "looptype", "pingpong"));
    }
}