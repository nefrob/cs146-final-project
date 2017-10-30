using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour {
    // Trigger collider
    private CircleCollider2D myCollider;

	// Init vars
	void Start () {
        myCollider = GetComponent<CircleCollider2D>();
    }

    /* Handle collision with shield. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            Destroy(collision.gameObject);
        }
    }
}
