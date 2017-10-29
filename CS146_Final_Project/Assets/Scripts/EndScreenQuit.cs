/*
* File:        End Screen Quit
* Author:      Robert Neff
* Date:        10/28/17
* Description: Implements support to quit the application.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenQuit: MonoBehaviour {

    /* Quit the application */
    public void Quit() {
        Application.Quit();
    }
}
