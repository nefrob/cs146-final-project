using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonFire : MonoBehaviour {

    public string side;
    public GameObject cannonBalls;
    public float cannonSpeed = 10f;
    public float fireTime = 2f;
    private double timePassed = 0;

    void Update()
    {
        if (timePassed > fireTime)
        {
            if (side.Contains("left"))
            {
                Vector3 pos = transform.position;
                pos.y += 1;
                GameObject bullet = Instantiate(cannonBalls, pos, transform.rotation) as GameObject;
                bullet.GetComponent<Rigidbody>().AddForce(-cannonSpeed, 0, 0, ForceMode.Impulse);
            }

            if (side.Contains("right"))
            {
                Vector3 pos = transform.position;
                pos.y += 1;
                GameObject bullet = Instantiate(cannonBalls, pos, transform.rotation) as GameObject;
                bullet.GetComponent<Rigidbody>().AddForce(cannonSpeed, 0, 0, ForceMode.Impulse);
            }

            timePassed = 0;
        }
        timePassed = timePassed + Time.deltaTime;
    }


    // Use this for initialization
    void Start () {
		
	}
	
}
