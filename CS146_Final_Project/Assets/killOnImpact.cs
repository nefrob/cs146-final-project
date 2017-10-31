using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killOnImpact : MonoBehaviour {

    void Start() {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            Destroy(this.gameObject, 0.02f);
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
