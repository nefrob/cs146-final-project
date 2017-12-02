using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backAndForthHover : MonoBehaviour {
    public string direction = "backward"; //Can be forward/backwards
    public double changeDirTime = 3;
    public Transform flipableBody;
    public float speed = 6f;
    public bool canFlip = true;
    private double timePassed = 0;
    private Rigidbody2D enemy;
    private bool dead;

    private EnemyRespawn respawnScript;
    private Vector2 speedBefore;

    // Use this for initialization
    void Start () {
        enemy = GetComponent<Rigidbody2D>();
        timePassed = 10000;
        respawnScript = GetComponent<EnemyRespawn>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead && respawnScript.isDead) return;

        if (!dead && respawnScript.isDead)
        {
            dead = true;
            speedBefore = enemy.velocity;
            enemy.velocity = Vector2.zero;
            return;
        }
        if (dead && !respawnScript.isDead)
        {
            dead = false;
            enemy.velocity = speedBefore;
        }


        Vector3 theScale = flipableBody.localScale;
        if (timePassed > changeDirTime)
        {
            if (direction == "backward")
            {
                direction = "forward";
                //enemy.AddForce(Vector2.right* -speed,ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * -speed;  
            }
            else
            {
                direction = "backward";
                //enemy.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * speed;
            }

            if (canFlip)
            {
                theScale.z *= -1;
                flipableBody.localScale = theScale;
            }
            timePassed = 0;
        }
        timePassed = timePassed + Time.deltaTime;
    }
    }
