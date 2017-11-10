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
    [SerializeField] private AudioClip[] celebrations;
    [SerializeField] private AudioClip[] taunts;

    /* Init audio items. */
    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
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
        playerSource.PlayOneShot(generalCommentary[Random.Range(0, generalCommentary.Length)]);
    }

    /* Plays random celebration dialogue sound. */
    public void playCelebrationSound()
    {
        playerSource.PlayOneShot(celebrations[Random.Range(0, celebrations.Length)]);
    }

    /* Plays random taunt dialogue sound. */
    public void playTauntSound()
    {
        playerSource.PlayOneShot(taunts[Random.Range(0, taunts.Length)]);
    }
}
