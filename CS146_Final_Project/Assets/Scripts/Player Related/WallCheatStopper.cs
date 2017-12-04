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
    private string messageBefore = "";

    /* Init vars. */
	void Start () {
        uiScript = FindObjectOfType<UIHandler>();
    }

    /* Check if collided and display message. */
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Ball") return;

        messageBefore = uiScript.messageText.text;
        uiScript.displayMessage(messages[Random.Range(0, messages.Length)]);
        Invoke("changeMsgBack", 1.75f);
    }

    /*Switches message back. */
    private void changeMsgBack()
    {
        uiScript.displayMessage(messageBefore);
        Invoke("hide", 2.0f);
    }

    /* Hides message thorugh invoke. */
    private void hide()
    {
        uiScript.hideMessage();
    }
}
