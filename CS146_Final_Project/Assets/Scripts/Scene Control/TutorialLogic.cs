using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour {
    // UI
    private UIHandler ui;
    [SerializeField] private string[] messages;
    // Actions
    private int actionCount = 0;
    private int distanceMarker = 0;

    // Colliders
    public GameObject[] blockers;
    public GameObject[] cheatRecognizers;
    public GameObject arrowsStart;
    public GameObject arrowsUp;
    public GameObject arrowsEnd;
    // Time
    private int currMsg = 0;
    private PlayerController player;

    /* Get objects. */
    void Start () {
        ui = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerController>();
        Invoke("displayNextText", 1.0f);
	}
	
	/* Get action updates. */
	void Update () {
        if (actionCount == 8) return;

        if (player.isDeadState())
        {
            ui.hideMessage();
            return;
        }

        // Check if moved
        if (actionCount == 0 && Input.GetAxis("Horizontal") != 0.0f)
        {
            actionCount++;
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
            blockers[0].SetActive(false);
            cheatRecognizers[0].SetActive(false);
            arrowsStart.SetActive(true);
        }
        // Check if jumped
        if (actionCount == 1 && Input.GetAxis("Jump") != 0)
        {
            actionCount++;
            Invoke("hide", 0.2f);
        }
        // Display next message when reach desired distance
        if (player.transform.position.x > 64 && actionCount == 2 && distanceMarker == 0)
        {
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
            distanceMarker++;
        }
        // Check if threw ball
        if (actionCount == 2 && Input.GetButton("Fire1") && distanceMarker == 1)
        {
            actionCount++;
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
        }
        // Check if dropped ball
        if (actionCount == 3 && Input.GetKey(KeyCode.W))
        {
            actionCount++;
            Invoke("hide", 0.2f);
            blockers[1].SetActive(false);
            cheatRecognizers[1].SetActive(false);
            arrowsUp.SetActive(true);
        }
        // Display next message when reach desired distance
        if (player.transform.position.x > 105 && actionCount == 4 && distanceMarker == 1)
        {
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
            distanceMarker++;
        }
        // Check if shielded
        if (actionCount == 4 && Input.GetMouseButton(1) && distanceMarker == 2)
        {
            actionCount++;
            Invoke("displayNextText", 1.0f);
            Invoke("hide", 0.2f);
        }
        // Check if crouched
        if (actionCount == 5 && Input.GetKey(KeyCode.S))
        {
            actionCount++;
            Invoke("hide", 0.2f);
            blockers[2].SetActive(false);
            cheatRecognizers[2].SetActive(false);
        }
        // Check if scrolled balls
        if (player.getNumBallsFound() > 1 && actionCount == 6)
        {
            Invoke("hide", 0.2f);
            Invoke("displayNextText", 1.0f);
            actionCount++;
        }
        // Check if scrolled balls
        if (actionCount == 7 && Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            actionCount++;
            hide();
            blockers[3].SetActive(false);
            cheatRecognizers[2].SetActive(false);
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
