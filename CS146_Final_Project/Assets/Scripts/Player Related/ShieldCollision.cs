/*
* File:        Shield Collision
* Author:      Robert Neff
* Date:        11/05/17
* Description: Handles collisions with the shield.
*/

using UnityEngine;

public class ShieldCollision : MonoBehaviour {
    // Missile hit deduction cost
    [Range(0.0f, 1.0f)] public float missileCost = 0.34f;
    [Range(0.0f, 1.0f)] public float laserCost = 0.1f;
    // Update shield ui and player status
    [SerializeField] private PlayerController playerScript;
    [SerializeField] private UIHandler uiScript;
    [SerializeField] private CameraShake shakeScript;
    // Impact points
    [SerializeField] public int collisionPoints = 2;

    /* Handle collision with shield. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            if (collision.gameObject.name.Substring(0, 9) == "LaserShot") // less cost and value
            {
                uiScript.shieldBarSlider.value -= laserCost;
                playerScript.updateScore(collisionPoints / 2, true);
            }
            else
            {
                uiScript.shieldBarSlider.value -= missileCost;
                playerScript.updateScore(collisionPoints, true);
            }
            if (uiScript.shieldBarSlider.value < 0) uiScript.shieldBarSlider.value = 0;
            playerScript.playExplosionSound();
            shakeScript.shakeScreen();
            Destroy(collision.gameObject);
        }
    }
}
