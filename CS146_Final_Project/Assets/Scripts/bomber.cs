using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomber : MonoBehaviour {
    public GameObject rockets;        
    public float fireTime = 2f;
    public float detectionRange = 6f;
    private Transform player;
    private float timePassed = 0;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        float range = Vector2.Distance(transform.position, player.transform.position);
        Vector3 directionToTarget = transform.position - player.transform.position;
        if (range <= detectionRange)
        {
            if (timePassed > fireTime)
            {
                Vector2 pos = transform.position;
                pos.y -= 1.2f;
                GameObject bomb = Instantiate(rockets,pos, transform.rotation) as GameObject;
                timePassed = 0;
            }
            timePassed = timePassed + Time.deltaTime;
        }
        
    }
}
