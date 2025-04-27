using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to your AudioMixer
    public Slider musicSlider;    // Slider for music volume
    public Slider sfxSlider;      // Slider for SFX volume
    public GameObject Settings;
    private bool isSettingsOpen = false;

    private void Start()
    {
        // Load saved preferences
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add listeners for slider changes
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Update()
    {
        if (isSettingsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingsOpen = false;
            Settings.SetActive(false);
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); // Convert to decibels
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20); // Convert to decibels
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.UIClickSFX); // Play click sound
        if (!isSettingsOpen)
        {
            isSettingsOpen = true;
            Settings.SetActive(true);
        }
        else
        {
            isSettingsOpen= false;
            Settings.SetActive(false);
        }
    }

    public void TestSFXButton()
    {
        AudioManager.instance.PlaySoundSFX(AudioManager.instance.pistolFireSFX);
    }
}
