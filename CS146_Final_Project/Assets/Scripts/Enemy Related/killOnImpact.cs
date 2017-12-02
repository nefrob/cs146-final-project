
using UnityEngine;

public class killOnImpact : MonoBehaviour
{
    public GameObject explosion;
    public bool onPlatformImpact = false;
    private PlayerController playerScript;
    private CameraShake shakeScript;
    private PlayerAudio playerAudio;
    private EnemyRespawn respawnScript;
    public bool yieldsImpactPoints = true;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
        shakeScript = FindObjectOfType<CameraShake>();
        playerAudio = FindObjectOfType<PlayerAudio>();
        respawnScript = GetComponent<EnemyRespawn>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().Die();
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0.02f);
        }
        else if (other.gameObject.tag == "Ball")
        {
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            if (other.tag != "Missile")
            {
                if (respawnScript != null && !respawnScript.hasDiedBefore)
                     playerScript.updateScore(10);
                playerAudio.playCelebrationSound();
            }
            shakeScript.shakeScreen(0.5f, 0.7f);

            if (other.tag == "Missile") Destroy(gameObject, 0.02f);
            else if (respawnScript != null) respawnScript.disableEnemy();
        }
        else if (onPlatformImpact && other.gameObject.tag == "Ground")
        {
            if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0.02f);
        }
    }
}