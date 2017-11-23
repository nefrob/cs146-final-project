using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketManager : MonoBehaviour
{
    //
    public GameObject rockets;
    public GameObject rocketsTargeted;
    public SimpleSentinel enemyControl;
    public Animation shoot;

    //Basic enemy controls #Not in use while random is checked (either of them)
    public float shootingSpeed = 10f;
    public float fireTime = 3f;
    

    //Random Shooting abilities
    public bool randomizeShootingTime =  false;
    public bool randomizeShootingSpeed = false;
    public float minRandInterval = 0.5f;
    public float maxRandInterval = 2.5f;
    public float minRandSpeed = 8f;
    public float maxRandSpeed = 20f;

    //Private internal control variables
    private PlayerController playerScript;
    private double timePassed = 0;
    private bool firingAllowed = false;
    private int counter = 0;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
        if (randomizeShootingTime) fireTime = Random.Range(minRandInterval,maxRandInterval);
    }

    void Update()
    {
        //Firing given Timer
        if (firingAllowed)
        {
            if (timePassed > fireTime)
            {
                fire();
                counter++;
                timePassed = 0;
                if (randomizeShootingTime) fireTime = Random.Range(minRandInterval, maxRandInterval);
            }
            timePassed = timePassed + Time.deltaTime;
        }
    }

    public void startFiring()
    {
        if (!firingAllowed)fire(); 
        firingAllowed = true;
    }

    public void stopFiring()
    {
        firingAllowed = false;
    }

    private void fire()
    {
        if (playerScript.isDeadState()) return;
        shoot.Play();
        if (randomizeShootingSpeed) shootingSpeed = Random.Range(minRandSpeed, maxRandSpeed);
        if (enemyControl.getDirection() == "left") createRocket(shootingSpeed*-1);
        else createRocket(shootingSpeed);
    }

    private void createRocket(float shootingSpeed)
    {
        Vector2 pos = transform.position;
        pos.y += 2.2f;
        GameObject bullet = Instantiate(rockets, pos, transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed, ForceMode2D.Impulse);
        Vector3 theScale = bullet.transform.localScale;
        if (shootingSpeed < 0) theScale *= -1;
        bullet.transform.localScale = theScale;
    }

    //TODO : IMPLEMENT IN MISSILE
    private void fireTargeted() {
        shoot.Play();
        if (enemyControl.getDirection() == "right")
        {
            Vector2 pos = transform.position;
            pos.y += 1.9f;
            GameObject bullet = Instantiate(rocketsTargeted, pos, transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 6, ForceMode2D.Impulse);
            Vector3 theScale = bullet.transform.localScale;
            bullet.transform.localScale = theScale;
        }

        else
        {
            Vector2 pos = transform.position;
            pos.y += 1.9f;
            GameObject bullet = Instantiate(rocketsTargeted, pos, transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -6, ForceMode2D.Impulse);
            Vector3 theScale = bullet.transform.localScale;
            theScale.z *= -1;
            bullet.transform.localScale = theScale;
        }
    }
}
