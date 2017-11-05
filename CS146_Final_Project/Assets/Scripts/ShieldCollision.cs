using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour {
    // Missile hit deduction cost
    [Range(0.0f, 1.0f)] public float cost = 0.3f;   
    // Change shield health in player script
    private PlayerController playerScript;

    // Get player script
    private void Start()
    {
        
    }

    /* Handle collision with shield. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            playerScript = FindObjectOfType<PlayerController>();
            playerScript.shieldBarSlider.value -= cost;
            if (playerScript.shieldBarSlider.value < 0) playerScript.shieldBarSlider.value = 0;
            playerScript.playExplosionSound();
            playerScript.updateScore(1);
            Destroy(collision.gameObject);
        }
    }
}
