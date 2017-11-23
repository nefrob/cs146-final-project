using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour {
    // Audio
    private PlayerAudio playerAudio;
    // UI
    private UIHandler ui;
    [SerializeField] private string[] messages;
    // Actions
    private bool moved = false;
    private bool jumped = false;
    private bool threwBall = false;
    private bool droppedBall = false;
    private bool pickedUpBall = false;
    private bool shielded = false;
    private bool scrolled = false;

    private bool distanceDisplay1 = false; // so bad style :(
    private bool distanceDisplay2 = false;
    private bool ballsChanged = false;

    // Colliders
    public GameObject[] blockers;
    public GameObject arrowsStart;
    public GameObject arrowsUp;
    public GameObject arrowsEnd;
    // Time
    private float timer = 0.0f;
    private int currMsg = 0;
    private PlayerController player;

    /* Get objects. */
    void Start () {
        playerAudio = FindObjectOfType<PlayerAudio>();
        ui = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerController>();
        Invoke("displayNextText", 1.0f);
	}
	
	/* Get action updates. */
	void Update () {
        if (player.isDeadState())
        {
            ui.hideMessage();
            return;
        }

        // Bad style but need to be done quick so leave
        if (!moved && Input.GetAxis("Horizontal") != 0.0f)
        {
            moved = true;
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
            blockers[0].SetActive(false);
            arrowsStart.SetActive(true);
        }
        if (!jumped && Input.GetAxis("Jump") != 0 && moved)
        {
            jumped = true;
            Invoke("hide", 0.2f);
        }

        if (player.transform.position.x > 64 && !threwBall && !distanceDisplay1)
        {
            Invoke("displayNextText", 0.0f);
            distanceDisplay1 = true;
        }

        if (!threwBall && Input.GetKey(KeyCode.Q) && moved && jumped)
        {
            threwBall = true;
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.5f);
        }
        if (!pickedUpBall && Input.GetKey(KeyCode.LeftShift) && moved && jumped && threwBall)
        {
            pickedUpBall = true;
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.5f);
        }
        if (!droppedBall && Input.GetKey(KeyCode.E) && moved && jumped && threwBall && pickedUpBall)
        {
            droppedBall = true;
            Invoke("hide", 0.2f);
            blockers[1].SetActive(false);
            arrowsUp.SetActive(true);
        }

        if (player.transform.position.x > 105 && !shielded && !distanceDisplay2)
        {
            Invoke("displayNextText", 0.0f);
            distanceDisplay2 = true;
        }

        if (!shielded && (player.transform.position.x > 105) && Input.GetMouseButton(1) && 
            moved && jumped && threwBall && droppedBall && pickedUpBall)
        {
            shielded = true;
            Invoke("hide", 0.2f);
            blockers[2].SetActive(false);
        }

        if (player.getNumBallsFound() > 1 && !scrolled && !ballsChanged)
        {
            Invoke("displayNextText", 0.0f);
            ballsChanged = true;
        }

        if (!scrolled && Input.GetAxis("Mouse ScrollWheel") != 0.0f && moved && jumped && threwBall 
            && droppedBall && pickedUpBall && shielded)
        {
            scrolled = true;
            Invoke("hide", 0.2f);
            blockers[3].SetActive(false);
            arrowsEnd.SetActive(true);
        }
    }

    /* Hides message thorugh invoke. */
    private void hide()
    {
        ui.hideMessage();
    }

    /* Displays next message. */
    private void displayNextText()
    {
        ui.displayMessage(messages[currMsg]);
        currMsg++;
    }
}
