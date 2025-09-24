using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro

public class AudioSettings : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterVolumeSlider;  // Slider for master volume
    public Slider musicVolumeSlider;   // Slider for music volume
    public Slider sfxVolumeSlider;     // Slider for SFX volume
    public Toggle muteToggle;          // Mute toggle

    [Header("Audio Sources")]
    public AudioSource mainAudioSource;  // AudioSource for main audio
    public AudioSource musicAudioSource; // AudioSource for music
    public AudioSource sfxAudioSource;   // AudioSource for SFX

    [Header("Default Values")]
    public float defaultMasterVolume = 1f; // Default master volume (0-1)
    public float defaultMusicVolume = 0.8f; // Default music volume (0-1)
    public float defaultSFXVolume = 0.8f; // Default SFX volume (0-1)

    void Start()
    {
        // Set default values for sliders
        masterVolumeSlider.value = defaultMasterVolume;
        musicVolumeSlider.value = defaultMusicVolume;
        sfxVolumeSlider.value = defaultSFXVolume;

        // Initialize the mute toggle state based on current volumes
        muteToggle.isOn = masterVolumeSlider.value == 0f;

        // Add listeners to handle value changes
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        // Apply initial volume settings
        ApplyVolumeSettings();
    }

    // Method to handle master volume changes
    void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;  // This controls the global master volume
        ApplyVolumeSettings();
    }

    // Method to handle music volume changes
    void OnMusicVolumeChanged(float value)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = value;
        }
    }

    // Method to handle SFX volume changes
    void OnSFXVolumeChanged(float value)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = value;
        }
    }

    // Method to handle mute toggle changes
    void OnMuteToggleChanged(bool isMuted)
    {
        if (isMuted)
        {
            masterVolumeSlider.value = 0f; // Mute the volume
        }
        else
        {
            masterVolumeSlider.value = defaultMasterVolume; // Restore default volume
        }
    }

    // Method to apply the volume settings based on all sliders
    private void ApplyVolumeSettings()
    {
        // Adjust the volume for each audio source based on the master volume slider
        if (mainAudioSource != null)
        {
            mainAudioSource.volume = masterVolumeSlider.value;
        }

        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolumeSlider.value * masterVolumeSlider.value;
        }

        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = sfxVolumeSlider.value * masterVolumeSlider.value;
        }
    }

    // Optionally, create a method to apply the changes (if you want to delay applying)
    public void ApplySettings()
    {
        // Save the settings, apply them, etc.
        // You can save these settings to PlayerPrefs to persist between sessions
    }
}
