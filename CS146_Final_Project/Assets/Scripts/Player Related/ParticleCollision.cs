using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour {

    //Can be added to variables

    public Transform effect;

	// Can be added to 'start' function
	void Start () {
        effect.GetComponent<ParticleSystem>().enableEmission = false;
	}


    /* Can be integrated into section on Dodgeball script */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            // Collide with ground
            effect.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(stopEffect());
        }
    }

    IEnumerator stopEffect()
    {
        yield return new WaitForSeconds(.5f);

        effect.GetComponent<ParticleSystem>().enableEmission = false;
    }
}
