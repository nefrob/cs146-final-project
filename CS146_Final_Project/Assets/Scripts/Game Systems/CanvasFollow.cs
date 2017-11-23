/*
* File:        Canvas Follow
* Author:      Robert Neff
* Date:        11/05/17
* Description: Makes the current object follow the player's x and y
*              positions with offset specified.
*/

using UnityEngine;

public class CanvasFollow : MonoBehaviour { 
    // Get player position
    public Transform player;
    // Position offsets
    [SerializeField] private float x_offset;
    [SerializeField] private float y_offset;
    [SerializeField] private float z_pos;

    // Update position of UI to track player
    void Update () {
        Vector3 temp = player.position;
        temp.x += x_offset;
        temp.y += y_offset;
        temp.z += z_pos;
        transform.position = temp;
	}
}
