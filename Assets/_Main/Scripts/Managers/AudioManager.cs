using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer Groups")]
    public UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;

    public static AudioManager instance;

    public AudioSource audioSourceSFX;
    public AudioSource audioSourceMenuMusic;
    public AudioSource audioSourceLevelMusic;

    private int sfxSourceCount = 20;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSFXIndex = 0;

    // Cooldowns
    private float lastGemPickUpTime = -1999;
    private float lastEnemyTakeDamageTime = -1999;
    private float lastEnemyDeathTime = -1999;

    [SerializeField] private float gemPickUpCooldown = 0.1f;
    [SerializeField] private float enemyTakeDamageCooldown = 0.1f;
    [SerializeField] private float enemyDeathCooldown = 0.1f;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    [Header("Weapon SFX")]
    public AudioClip[] bowAndArrowFireSFX;
    public AudioClip[] spearFireSFX;
    public AudioClip[] dartFireSFX;
    public AudioClip[] slingShotFireSFX;
    public AudioClip[] pistolFireSFX;
    public AudioClip[] shotgunFireSFX;
    public AudioClip[] rocketFireSFX;
    public AudioClip[] rocketExplosionSFX;
    public AudioClip[] grenadeSFX;
    public AudioClip[] grenadeExplodeSFX;
    public AudioClip[] slingShotSFX;
    public AudioClip[] orbitalStrikeSFX;

    [Header("Enemy SFX")]
    public AudioClip[] hammerheadSFX;
    public AudioClip[] enemyTakeDamageSFX;
    public AudioClip[] enemyDeathSFX;

    [Header("UI")]
    public AudioClip[] UIClickSFX;
    public AudioClip[] UIHoverSFX;
    public AudioClip[] LevelUpSFX;
    public AudioClip[] BoatUnlockSFX;
    public AudioClip[] NotEnoughMoneySFX;

    [Header("DropSFX")]
    public AudioClip[] wrenchPickUpSFX;
    public AudioClip[] magnetPickUpSFX;
    public AudioClip[] damagePickUpSFX;
    public AudioClip[] coinPickUpSFX;
    public AudioClip[] gemPickUp;

    private int lastPlayedIndex = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitializeSFXSources();
        PlayMainMenuMusic();
    }

    private void Start()
    {
        // When the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;
    }
    private void InitializeSFXSources()
    {
        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixerGroup; // <- THIS LINE IS CRITICAL
            sfxSources.Add(source);
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == 1)
        {
            PlayLevelMusic();
        }
        else
        {
            PlayMainMenuMusic();
        }
    }

    public void PlayGemPickUpSound()
    {
        if (Time.time - lastGemPickUpTime >= gemPickUpCooldown)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, gemPickUp.Length);
            } while (randomIndex == lastPlayedIndex && gemPickUp.Length > 1);

            PlaySoundSFX(gemPickUp);
            lastGemPickUpTime = Time.time;
        }
    }

    public void PlayEnemyTakeDamageSound()
    {
        if (Time.time - lastEnemyTakeDamageTime >= enemyTakeDamageCooldown)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, enemyTakeDamageSFX.Length);
            } while (randomIndex == lastPlayedIndex && enemyTakeDamageSFX.Length > 1);

            PlaySoundSFX(enemyTakeDamageSFX);
            lastEnemyTakeDamageTime = Time.time;
        }
    }

    public void PlayEnemyDeathSound()
    {
        if (Time.time - lastEnemyDeathTime >= enemyDeathCooldown)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, enemyDeathSFX.Length);
            } while (randomIndex == lastPlayedIndex && enemyDeathSFX.Length > 1);

            PlaySoundSFX(enemyDeathSFX);
            lastEnemyDeathTime = Time.time;
        }
    }


    public void PlaySoundSFX(AudioClip[] soundFXArray)
    {
        if (soundFXArray.Length == 0) return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, soundFXArray.Length);
        } while (randomIndex == lastPlayedIndex && soundFXArray.Length > 1);

        lastPlayedIndex = randomIndex;

        AudioSource currentSource = sfxSources[currentSFXIndex];
        currentSource.PlayOneShot(soundFXArray[randomIndex]);

        currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Count;
    }

    private void PlayMainMenuMusic()
    {
        audioSourceMenuMusic.clip = menuMusic;
        audioSourceMenuMusic.Play();
        audioSourceLevelMusic.Stop();
    }

    private void PlayLevelMusic()
    {
        audioSourceLevelMusic.clip = levelMusic;
        audioSourceLevelMusic.Play();
        audioSourceMenuMusic.Stop();
    }
}
