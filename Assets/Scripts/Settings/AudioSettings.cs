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
    }

    // Method to handle master volume changes
    void OnMasterVolumeChanged(float value)
    {
        // Apply the master volume to all audio sources
        AudioListener.volume = value;
    }

    // Method to handle music volume changes
    void OnMusicVolumeChanged(float value)
    {
        // You can assign this to your background music audio source
        // Example: musicAudioSource.volume = value;
    }

    // Method to handle SFX volume changes
    void OnSFXVolumeChanged(float value)
    {
        // You can assign this to your SFX audio source
        // Example: sfxAudioSource.volume = value;
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

    // Optionally, create a method to apply the changes (if you want to delay applying)
    public void ApplySettings()
    {
        // Save the settings, apply them, etc.
        // You can save these settings to PlayerPrefs to persist between sessions
    }
}