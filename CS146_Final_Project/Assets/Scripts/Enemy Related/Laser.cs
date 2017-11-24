using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public Transform m_cannonRot;
	public Transform m_muzzle;
	public GameObject m_shotPrefab;
	public Texture2D m_guiTexture;

    //Basic enemy controls #Not in use while random is checked (either of them)
    public float shootingSpeed = 100f;
    public float fireTime = 0.3f;
    public float detectionRange = 12f;

    //Random Shooting abilities
    public bool randomizeShootingTime = true;
    public bool randomizeShootingSpeed = false;
    public float minRandInterval = 0.5f;
    public float maxRandInterval = 2.5f;
    public float minRandSpeed = 8f;
    public float maxRandSpeed = 20f;

    //Private internal control variables
    private PlayerController playerScript;
    private double timePassed = 0;
    private bool firingAllowed = true;
    private int counter = 0;
    private Transform player;

    // Audio
    private AudioSource source;


    private void Start()
    {
        player = GameObject.Find("FBX/Hips").GetComponent<Transform>();
        if (randomizeShootingTime) fireTime = Random.Range(minRandInterval, maxRandInterval);

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.LookAt(player, Vector3.forward);
        //Firing given Timer
        if (Vector2.Distance(transform.position, player.transform.position) < detectionRange)
        {
            if (timePassed > fireTime)
            {
                fire();
                timePassed = 0;
                if (randomizeShootingTime) fireTime = Random.Range(minRandInterval, maxRandInterval);
            }
            timePassed = timePassed + Time.deltaTime;
        }
    }
    private void fire()
    {
        //if (playerScript.isDeadState()) return;
        //shoot.Play(); // Need new sound
        GameObject go = Instantiate(m_shotPrefab, m_muzzle.position, Quaternion.identity);
        go.transform.GetChild(0).rotation = m_muzzle.rotation;
        source.Play();
        go.GetComponent<Rigidbody2D>().AddForce(go.transform.GetChild(0).forward * shootingSpeed, ForceMode2D.Impulse);
        GameObject.Destroy(go, 3f);
    }

}
