using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class rocketManager : MonoBehaviour
{
    public GameObject rockets;
    public GameObject rocketsTargeted;
    public SimpleSentinel enemyControl;
    public float cannonSpeed = 10f;
    public float fireTime = 3f;
    public Animation shoot;
    private PlayerController playerScript;
    private double timePassed = 0;
    private bool firingAllowed = false;
    private int counter = 0;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
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

    //Organize these up
    private void fire()
    {
        if (playerScript.isDeadState()) return;
        shoot.Play();
        if (enemyControl.getDirection() == "forward")
        {
            Vector2 pos = transform.position;
            pos.y += 2.2f;
            GameObject bullet = Instantiate(rockets, pos, transform.rotation) as GameObject;            
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 6, ForceMode2D.Impulse);
            Vector3 theScale = bullet.transform.localScale;
            bullet.transform.localScale = theScale;
        }

        else
        {
            Vector2 pos = transform.position;
            pos.y += 2.2f;
            GameObject bullet = Instantiate(rockets, pos, transform.rotation) as GameObject;          
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -6, ForceMode2D.Impulse);
            Vector3 theScale = bullet.transform.localScale;
            theScale.z *= -1;
            bullet.transform.localScale = theScale;
        }
    }

    private void fireTargeted() {
        shoot.Play();
        if (enemyControl.getDirection() == "forward")
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
