using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killOnImpact : MonoBehaviour
{
    public GameObject explosion;
    public bool onPlatformImpact = false;
    private PlayerController playerScript;
    private CameraShake shakeScript;
    private PlayerAudio playerAudio;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
        shakeScript = FindObjectOfType<CameraShake>();
        playerAudio = FindObjectOfType<PlayerAudio>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0.02f);
        }
        else if (other.gameObject.tag == "Ball")
        {
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            playerScript.updateScore(10);
            playerAudio.playCelebrationSound();
            shakeScript.shakeScreen(0.5f, 0.7f);
            Destroy(gameObject, 0.02f);
        }
        else if (onPlatformImpact && other.gameObject.tag == "Ground")
        {
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0.02f);
        }
    }
}