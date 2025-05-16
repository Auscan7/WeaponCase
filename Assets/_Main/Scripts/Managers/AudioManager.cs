using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSourceSFX;
    public AudioSource audioSourceMenuMusic;
    public AudioSource audioSourceLevelMusic;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    [Header("Weapon SFX")]
    public AudioClip[] bowAndArrowFireSFX;
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

        PlayMainMenuMusic();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // When the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;
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

    public void PlaySoundSFX(AudioClip[] soundFXArray)
    {
        if (audioSourceSFX != null && soundFXArray.Length > 0)
        {
            // Choose a random index, ensuring it's not the same as the last played
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, soundFXArray.Length);
            } while (randomIndex == lastPlayedIndex && soundFXArray.Length > 1);

            lastPlayedIndex = randomIndex;

            // Play the randomly selected sound
            audioSourceSFX.PlayOneShot(soundFXArray[randomIndex]);
        }
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
