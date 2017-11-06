using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killOnImpact : MonoBehaviour
{
    public GameObject explosion;
    public bool onPlatformImpact = false;
    private PlayerController playerScript;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
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
            Destroy(this.gameObject, 0.02f);
        }
        else if (onPlatformImpact)
        {
            GameObject boom = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            Destroy(this.gameObject, 0.02f);
        }
    }
}