using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle shakeToggle;
    public GameObject Settings;
    private bool isSettingsOpen = false;
    private AudioManager audioManager;
    private void Start()
    {
        audioManager = AudioManager.instance;
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        bool shakeEnabled = PlayerPrefs.GetInt("CameraShakeEnabled", 1) == 1;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetCameraShake(shakeEnabled);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        shakeToggle.isOn = shakeEnabled;
        shakeToggle.onValueChanged.AddListener(SetCameraShake);
    }


    private void Update()
    {
        if (isSettingsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingsOpen = false;
            Settings.SetActive(false);
        }
    }

    public void SetCameraShake(bool isEnabled)
    {
        PlayerPrefs.SetInt("CameraShakeEnabled", isEnabled ? 1 : 0);
        CameraShakeManager.Instance.ShakeEnabled = isEnabled;
        audioManager.PlaySoundSFX(audioManager.UIClickSFX); // Play click sound
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
        audioManager.PlaySoundSFX(audioManager.UIClickSFX); // Play click sound
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
        if(audioManager != null)
            audioManager.PlaySoundSFX(audioManager.pistolFireSFX);
    }
}
