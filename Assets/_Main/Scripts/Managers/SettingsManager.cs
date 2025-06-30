using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle shakeToggle;
    public GameObject Settings;
    private bool isSettingsOpen = false;
    private AudioManager audioManager;
    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        SetMasterVolume(masterSlider.value);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);

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

    public void SetMasterVolume(float volume)
    {
        if (volume <= 0.05f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            float dbVolume = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("Master", dbVolume);
        }

        PlayerPrefs.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= 0.05f)
        {
            audioMixer.SetFloat("MusicVolume", -80f); // Unity’s mute threshold
        }
        else
        {
            float dbVolume = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("MusicVolume", dbVolume);
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.05f)
        {
            audioMixer.SetFloat("SFXVolume", -80f); // Fully mute
        }
        else
        {
            float dbVolume = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("SFXVolume", dbVolume);
        }

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
