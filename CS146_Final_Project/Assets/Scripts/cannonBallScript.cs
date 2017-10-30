using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallScript : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "BaseTile")
        {
            Destroy(gameObject);
        }
    }
}
