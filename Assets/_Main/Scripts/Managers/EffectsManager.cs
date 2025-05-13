using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;

    [Header("WeaponFireVFX")]
    public GameObject bulletVFX;
    public GameObject pelletVFX;
    public GameObject rocketVFX;

    [Header("WeaponHitVFX")]
    public GameObject bulletHitVFX;
    public GameObject orbitalVFX;
    public GameObject slingShotVFX;
    public GameObject grenadeVFX;

    [Header("VFX")]
    public GameObject enemyDeathVFX;
    public GameObject coinPickUpVFX;
    public GameObject healPickUpVFX;
    public GameObject damagePickUpVFX;

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

    public void PlayVFX(GameObject VFXToPlay, Vector2 spawnPos, Quaternion rotation, float duration = 4f)
    {
        if (VFXToPlay != null)
        {
            GameObject vfxInstance = Instantiate(VFXToPlay, spawnPos, rotation);
            Destroy(vfxInstance, duration);
        }
    }
}
