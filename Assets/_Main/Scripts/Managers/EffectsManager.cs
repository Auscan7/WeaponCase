using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    [Header("VFX")]
    public GameObject bulletVFX;
    public GameObject pelletVFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayVFX(GameObject VFXToPlay, Vector2 spawnPos, Quaternion rotation)
    {
        if (VFXToPlay != null)
        {
            Instantiate(VFXToPlay, spawnPos, rotation);
        }
    }
}
