/*
* File:        Player Audio
* Author:      Robert Neff
* Date:        11/21/17
* Description: Store player audio, trigger audio related events.
*/

using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    // Player sources
    public AudioSource playerSource;
    public AudioSource calloutSource;
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
    [SerializeField] private AudioClip[] pickupCommentary;
    [SerializeField] private string[] pickupMessages;
    [SerializeField] private AudioClip[] outCommentary;
    [SerializeField] private string[] outMessages;
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

    /* Template to play random callout sound. */
    private void playRandCallout(AudioClip[] sounds, string[] messages, bool useUI = true)
    {
        if (sounds.Length > 0 && !calloutSource.isPlaying)
        {
            int rand = Random.Range(0, sounds.Length);
            calloutSource.PlayOneShot(sounds[rand]);
            if (useUI) ui.setCalloutText(messages[rand]);
        }
    }

    /* Plays random comment dialogue sound. */
    public void playCommentSound()
    {
        playRandCallout(generalCommentary, commentMessages);
    }

    /* Plays random comment dialogue sound. */
    public void playPickupCommentSound()
    {
        playRandCallout(pickupCommentary, pickupMessages, false);
    }

    /* Plays random comment dialogue sound. */
    public void playOutCommentSound()
    {
        playRandCallout(outCommentary, outMessages, false);
    }

    /* Plays random celebration dialogue sound. */
    public void playCelebrationSound()
    {
        playRandCallout(celebrations, celebrationMessages);
    }

    /* Plays random taunt dialogue sound. */
    public void playTauntSound()
    {
        playRandCallout(taunts, tauntMessages);
    }
}
