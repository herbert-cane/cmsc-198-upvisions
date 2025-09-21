using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton to ensure only one MusicManager exists
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;  // AudioSource for background music
    public AudioSource sfxSource;              // AudioSource for sound effects

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;            // Music for Main Menu
    public AudioClip gameMusic;                // Music for gameplay
    public AudioClip buttonClickSFX;           // Sound effect for button click

    void Awake()
    {
        // Ensure that there is only one instance of MusicManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure the MusicManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // If there's another instance, destroy this one
        }
    }

    void Start()
    {
        // Ensure background music source is playing
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
        {
            Debug.Log("Starting background music...");
            PlayBackgroundMusic(mainMenuMusic); // Start the background music for the Main Menu by default
        }
    }

    // Method to play background music (loops)
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (backgroundMusicSource.isPlaying) // Stop any music currently playing
        {
            backgroundMusicSource.Stop();
        }

        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.loop = true; // Ensure music loops
        backgroundMusicSource.Play();
        Debug.Log("Now playing: " + musicClip.name);  // Debug log to confirm playback
    }

    // Method to stop the background music
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
            Debug.Log("Background music stopped.");
        }
    }

    // Method to play sound effects (non-looping)
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxClip); // Play sound effect once
            Debug.Log("Playing SFX: " + sfxClip.name); // Debug log for SFX
        }
    }
}