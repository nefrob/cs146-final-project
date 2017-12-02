using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomber : MonoBehaviour {
    public GameObject rockets;        
    public float fireTime = 2f;
    public bool randomBombsMode = false;
    public float detectionRange = 6f;
    private Transform player;
    private float timePassed = 0;

    private EnemyRespawn respawnScript;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<Transform>();
        respawnScript = GetComponent<EnemyRespawn>();
    }
	
	// Update is called once per frame
	void Update () {
        if (respawnScript.isDead) return;

        float range = Vector2.Distance(transform.position, player.transform.position);
        Vector3 directionToTarget = transform.position - player.transform.position;

        if (range <= detectionRange) {
            if (randomBombsMode)
            {
                if (timePassed > fireTime)
                {
                    dropBomb();
                    timePassed = 0;
                }
                timePassed = timePassed + Time.deltaTime;
            }
            else if (player.transform.position.x > -1 + transform.position.x && player.transform.position.x < 1 + transform.position.x)
            {
                if (timePassed > fireTime)
                {
                    dropBomb();
                    timePassed = 0;
                }               

            }
            else {
                timePassed = timePassed + Time.deltaTime;
            }
        }
    }

    private void dropBomb() {
        Vector2 pos = transform.position;
        pos.y -= 1.2f;
        GameObject bomb = Instantiate(rockets, pos, transform.rotation) as GameObject;
        bomb.GetComponent<killOnImpact>().yieldsImpactPoints = !respawnScript.hasDiedBefore;
    }
}
