using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour { 
    // Get player position
    public Transform player;
    // Position offsets
    [SerializeField] private float x_offset;
    [SerializeField] private float y_offset;

    // Update position of UI to track player
    void Update () {
        Vector3 temp = player.position;
        temp.x += x_offset;
        temp.y += y_offset;
        transform.position = temp;
	}
}
