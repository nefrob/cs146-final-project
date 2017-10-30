using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeSpanControl : MonoBehaviour {
    private double timePassed = 0;
    public double timeLimit = 10;

    // Update is called once per frame
    void Update () {
        if (timePassed > timeLimit)
        {
            Destroy(this.gameObject, 0.02f);
        }
        timePassed = timePassed + Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "player(1)")
        {
            //Destroy(this.gameObject, 0.02f);
        }
        else {
            //Destroy player
        }            
        
    }
}
