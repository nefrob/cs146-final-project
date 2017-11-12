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
    private Rigidbody2D player;
    public float speed = 10f;
    public rocketManager fireScript;
    [SerializeField]
    private GameObject explosion;

    private PlayerController playerScript;

    void Start(){
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        enemy = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    void Update () {
        bool movementEnabled = true;
        //Use the two store floats to create a new Vector2 variable movement.
        float range = Vector2.Distance(transform.position, player.transform.position);
        
        //Angle to player calculation
        Vector3 directionToTarget = player.transform.position - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        //Is in range and is looking at direction and is in angle

        if (range <= detectionRange &&
            (Vector3.Angle(transform.position - player.transform.position, transform.forward) <= 10 &&
            !Physics.Linecast(transform.position, player.transform.position)) && direction == "forward")
        {
            movementEnabled = false;
            fireScript.startFiring();
            enemy.velocity = Vector2.right * 0;
        }
        else if (range <= detectionRange &&
            (Vector3.Angle(player.transform.position - transform.position, transform.forward) <= 10 &&
            !Physics.Linecast(transform.position, player.transform.position)) && direction == "backward")
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
