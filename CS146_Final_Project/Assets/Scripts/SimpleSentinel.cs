/*
 Implements a very basic sentinel type with movement towards the sides
 *Should move firing capabilities
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSentinel : MonoBehaviour {
    public string direction = "forward"; //Can be forward/backwards
    public double changeDirTime = 3;
    public double detectionRange = 10;
    private double timePassed = 0;
    private Rigidbody2D enemy;
    public Rigidbody2D player;
    public float speed = 10f;
    public rocketManager fireScript;
    [SerializeField]
    private GameObject explosion;

    private PlayerController playerScript;

    void Start(){
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        enemy = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerController>();
    }

    void Update () {
        //Use the two store floats to create a new Vector2 variable movement.
        float range = Vector2.Distance(transform.position, player.transform.position);
        Vector3 directionToTarget = transform.position - player.transform.position;
        bool movementEnabled = true;
        float scalez = transform.localScale.z;

        //Is in range and is looking at direction
        if (range <= detectionRange && directionToTarget.x > 0 && direction == "forward")
        {
            movementEnabled = false;
            fireScript.startFiring();
            enemy.velocity = Vector2.right * 0;
        }
        else if (range <= detectionRange && directionToTarget.x < 0 && direction == "backward")
        {
            movementEnabled = false;
            fireScript.startFiring();
            enemy.velocity = Vector2.right * 0;
        }
        else {
            fireScript.stopFiring();
            movementEnabled = true;
        }
        
        Vector3 theScale = transform.localScale;
        if (timePassed > changeDirTime && movementEnabled)
            {
                if (direction == "backward")
                {
                    direction = "forward";
                    //enemy.AddForce(Vector2.right* -speed,ForceMode2D.Impulse);
                    enemy.velocity = Vector2.right*-speed;
                    theScale.z *= -1;
                    transform.localScale = theScale;
            }
                else
                {
                    direction = "backward";
                    //enemy.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                    enemy.velocity = Vector2.right * speed;
                    theScale.z *= -1;
                    transform.localScale = theScale;
            }
                    timePassed = 0;
            }
            timePassed = timePassed + Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 pos = transform.position;
            pos.y += 1.5f;
            Destroy(this.gameObject, 0.02f);
            GameObject boom = Instantiate(explosion, pos, transform.rotation) as GameObject;
            other.gameObject.GetComponent<PlayerController>().Die();
        } else if(other.gameObject.tag == "Ball")
        {
            GameObject boom = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            playerScript.updateScore(10);
            Destroy(this.gameObject, 0.02f);
        }
    }

    //_______________________________________________________________________________FOLLOW PLAYER________________________________________________________________________________
    public void followPlayer() {
        float range = Vector2.Distance(transform.position, player.transform.position);
        if (range <= 15f)
        {
            transform.Translate(Vector2.MoveTowards(transform.position, player.transform.position, range) * speed * Time.deltaTime);
        }
    }

    //_______________________________________________________________________________HELPERS______________________________________________________________________________________
    public string getDirection()
    {
        if (direction == "forward")
        {
            return "backward";
        }
        else
        {
            return "forward";
        }
        
    }
}
