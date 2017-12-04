/*
* File:        Enemy Respawn
* Author:      Robert Neff
* Date:        12/01/17
* Description: Allows for disabling of enemy before respawn after 
*              specified time period (instead of death by object destruction).
*/

using UnityEngine;
using UnityEngine.UI;

public class EnemyRespawn : MonoBehaviour {
    // Respawn variables
    [SerializeField] private float respawnTime = 5.0f;
    [SerializeField] private GameObject respawnUI;
    [SerializeField] private Text respawnStatus;
    [SerializeField] private Image respawnImg;
    [SerializeField] private GameObject container;
    private float value;
    private BoxCollider2D myCollider;
    private Rigidbody2D rb;
    public bool isDead;
    public bool hasDiedBefore;

    /* Get required info. */
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        value = respawnTime;
        hasDiedBefore = false;
    }
	
	/* Update respawn timer if necessary. */
	void Update () {
        if (!respawnUI.activeInHierarchy) return;

        value -= Time.deltaTime;
        respawnStatus.text = value.ToString("F1");
        respawnImg.fillAmount = value / respawnTime;
    }

    /* Disable on "death" by player and respawn after given amount of time. */
    public void disableEnemy(bool freezePos = false)
    {
        isDead = true;
        hasDiedBefore = true;
        if (freezePos)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        myCollider.enabled = false;
        setAlpha(0.45f);
        respawnUI.SetActive(true);
        value = 5.0f;
        respawnImg.fillAmount = value / respawnTime;

        Invoke("respawn", respawnTime);
    }

    /* Respawn enemy. */
    private void respawn()
    {
        isDead = false;
        myCollider.enabled = true;
        setAlpha(1.0f);
        respawnUI.SetActive(false);
    }

    /* Sets alpha value of gameobject's children (gameobj is just container). */
    private void setAlpha(float a)
    {
        MeshRenderer[] children = container.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < children.Length; i++)
        {
            Material childMat = children[i].GetComponent<Renderer>().material;
            children[i].GetComponent<Renderer>().material.color = 
                new Color(childMat.color.r, childMat.color.g, childMat.color.b, a);
        }
    }
}
