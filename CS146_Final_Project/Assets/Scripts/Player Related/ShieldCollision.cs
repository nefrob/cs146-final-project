/*
* File:        Shield Collision
* Author:      Robert Neff
* Date:        11/05/17
* Description: Handles collisions with the shield.
*/

using UnityEngine;

public class ShieldCollision : MonoBehaviour {
    // Missile hit deduction cost
    [Range(0.0f, 1.0f)] public float cost = 0.34f;
    // Update shield ui and player status
    private PlayerController playerScript;
    private UIHandler uiScript;
    private CameraShake shakeScript;
    // Impact points
    [SerializeField ]public int collisionPoints = 2;

    // Get ui script
    void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
        uiScript = FindObjectOfType<UIHandler>();
        shakeScript = FindObjectOfType<CameraShake>();
    }

    /* Handle collision with shield. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            uiScript.shieldBarSlider.value -= cost;
            if (uiScript.shieldBarSlider.value < 0) uiScript.shieldBarSlider.value = 0;
            playerScript.playExplosionSound();
            shakeScript.shakeScreen();
            playerScript.updateScore(collisionPoints);
            Destroy(collision.gameObject);
        }
    }
}
