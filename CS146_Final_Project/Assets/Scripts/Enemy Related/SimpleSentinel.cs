/*
 Implements a very basic sentinel type with movement towards the sides
 *Should move firing capabilities
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSentinel : MonoBehaviour {
    public string direction = "left"; //Can be right/left
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
        fireManager(ref movementEnabled);
        directionManager(movementEnabled);
        Debug.Log(GetFirstRaycastHit(transform.forward, (float)detectionRange).collider.gameObject.tag);
    }

    private void fireManager(ref bool movementEnabled) {
        //Use the two store floats to create a new Vector2 variable movement.
        float range = Vector2.Distance(transform.position, player.transform.position);

        //Is in range and is looking at direction and is in angle
        if (range <= detectionRange &&
            (GetFirstRaycastHit(-transform.forward, (float)detectionRange).collider.gameObject.tag == "Player") && direction == "right"){
            movementEnabled = false;
            fireScript.startFiring();
            enemy.velocity = Vector2.right * 0;
        } 

        else if (range <= detectionRange &&
            (GetFirstRaycastHit(transform.forward, (float)detectionRange).collider.gameObject.tag == "Player") && direction == "left") {
            movementEnabled = false;
            fireScript.startFiring();
            enemy.velocity = Vector2.right * 0;
        }
        else{
            fireScript.stopFiring();
            movementEnabled = true;
        }
    }

    private void directionManager(bool movementEnabled) {
        Vector3 theScale = transform.localScale;
        if (timePassed > changeDirTime && movementEnabled)
        {
            if (direction == "right")
            {
                //enemy.AddForce(Vector2.right* -speed,ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * -speed;
                theScale.z *= -1;
                transform.localScale = theScale;
                direction = "left";
            }
            else
            {
                //enemy.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * speed;
                theScale.z *= -1;
                transform.localScale = theScale;
                direction = "right";
            }
            timePassed = 0;
        }
        if (movementEnabled) {
            timePassed = timePassed + Time.deltaTime;
        }

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

    //______________________________________________________________________FOLLOW PLAYER_____________________________________________________________________________
    public void followPlayer() {
        float range = Vector2.Distance(transform.position, player.transform.position);
        if (range <= 15f)
        {
            transform.Translate(Vector2.MoveTowards(transform.position, player.transform.position, range) * speed * Time.deltaTime);
        }
    }

    //_________________________________________________________________________HELPERS________________________________________________________________________________
    public string getDirection(){
        return direction;
    }

    public RaycastHit2D GetFirstRaycastHit(Vector2 direction, float range)
    {
        //Check "Queries Start in Colliders" in Edit > Project Settings > Physics2D
        RaycastHit2D[] hits = new RaycastHit2D[2];
        hits = Physics2D.RaycastAll(transform.position, -direction, range);
        //hits[0] will always be the Collider2D you are casting from.
        if (hits.Length == 1) return hits[0];
        return hits[1];
    }
}
