/*
* File:        Cursor Set
* Author:      Robert Neff
* Date:        12/02/17
* Description: Sets the default cursor for the game.
*/

using UnityEngine;

public class CursorSet : MonoBehaviour {

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
