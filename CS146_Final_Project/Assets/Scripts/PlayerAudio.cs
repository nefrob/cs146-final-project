/*
* File:        Player Audio
* Author:      Robert Neff
* Date:        11/09/17
* Description: Store player audio, trigger audio related events.
*/

using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    // Player sources
    public AudioSource playerSource;
    public AudioSource shieldSource;
    // Player sounds
    public AudioClip jumpSound;
    public AudioClip explosionSound;
    public AudioClip pickupBall;
    public AudioClip dropSound;
    [SerializeField] private AudioClip[] throwSounds;
    public AudioClip shieldSound; // should be able to loop
    public AudioClip deathSound;
    public AudioClip fallingDeathSound;
    public AudioClip switchBall;
    [SerializeField] private AudioClip[] generalCommentary;
    [SerializeField] private string[] commentMessages;
    [SerializeField] private AudioClip[] celebrations;
    [SerializeField] private string[] celebrationMessages;
    [SerializeField] private AudioClip[] taunts;
    [SerializeField] private string[] tauntMessages;

    private UIHandler ui;

    /* Init audio items. */
    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
        ui = FindObjectOfType<UIHandler>();
    }

    /* Set shield clip. */
    void Start()
    {
        shieldSource.clip = shieldSound;
    }

    /* Plays random throw sound. */
    public void playThrowSound()
    {
        playerSource.clip = throwSounds[Random.Range(0, throwSounds.Length)];
        playerSource.Play();
    }

    /* Plays random comment dialogue sound. */
    public void playCommentSound()
    {
        if (generalCommentary.Length > 0)
        {
            int rand = Random.Range(0, generalCommentary.Length);
            playerSource.PlayOneShot(generalCommentary[rand]);
            ui.setCalloutText(commentMessages[rand]);
        }
    }

    /* Plays random celebration dialogue sound. */
    public void playCelebrationSound()
    {
        if (celebrations.Length > 0)
        {
            int rand = Random.Range(0, celebrations.Length);
            playerSource.PlayOneShot(celebrations[rand]);
            ui.setCalloutText(celebrationMessages[rand]);
        }
    }

    /* Plays random taunt dialogue sound. */
    public void playTauntSound()
    {
        if (taunts.Length > 0)
        {
            int rand = Random.Range(0, taunts.Length);
            playerSource.PlayOneShot(taunts[rand]);
            ui.setCalloutText(tauntMessages[rand]);
        }
    }
}
