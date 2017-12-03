/*
* File:        Wall Cheat Stopper
* Author:      Robert Neff
* Date:        12/02/17
* Description: Checks if balls has gone through illiegal surface, and winks
*              at the layer. Doesn't really stop cheating to make fun of tactic.
*/

using UnityEngine;

public class WallCheatStopper : MonoBehaviour {
    // Get ui to display message
    private UIHandler uiScript;
    [SerializeField] private string[] messages;

    /* Init vars. */
	void Start () {
        uiScript = FindObjectOfType<UIHandler>();
    }

    /* Check if collided and display message. */
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Ball") return;

        uiScript.displayMessage(messages[Random.Range(0, messages.Length)]);
        Invoke("hide", 1.5f);
    }

    /* Hides message thorugh invoke. */
    private void hide()
    {
        uiScript.hideMessage();
    }
}
