using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backAndForthHover : MonoBehaviour {
    public string direction = "backward"; //Can be forward/backwards
    public double changeDirTime = 3;
    public Transform flipableBody;
    public float speed = 6f;
    private double timePassed = 0;
    private Rigidbody2D enemy;
    // Use this for initialization
    void Start () {
        enemy = GetComponent<Rigidbody2D>();
        timePassed = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 theScale = flipableBody.localScale;
        if (timePassed > changeDirTime)
        {
            if (direction == "backward")
            {
                direction = "forward";
                //enemy.AddForce(Vector2.right* -speed,ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * -speed;
                theScale.z *= -1;
                flipableBody.localScale = theScale;
            }
            else
            {
                direction = "backward";
                //enemy.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                enemy.velocity = Vector2.right * speed;
                theScale.z *= -1;
                flipableBody.localScale = theScale;
            }
            timePassed = 0;
        }
        timePassed = timePassed + Time.deltaTime;
    }
    }
