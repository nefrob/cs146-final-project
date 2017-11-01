using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour {

    /* Handle collision with shield. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            Destroy(collision.gameObject);
        }
    }
}
