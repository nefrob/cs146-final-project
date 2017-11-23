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
            Destroy(this.gameObject, 0.02f);
            other.gameObject.GetComponent<PlayerController>().Die();
            GameObject boom = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        }
        else if (other.gameObject.tag == "Ball")
        {
            GameObject boom = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            playerScript.updateScore(10);
            playerAudio.playCelebrationSound();
            shakeScript.shakeScreen(0.5f, 0.7f);
            Destroy(this.gameObject, 0.02f);
        }
        else if (onPlatformImpact)
        {
            GameObject boom = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            Destroy(this.gameObject, 0.02f);
        }
    }
}