using UnityEngine;

public class PermenantUpgradeManager : MonoBehaviour
{
    public static PermenantUpgradeManager Instance;

    public int maxHealth;
    public float armor;
    public float damage;
    public float critDamage;
    public float moveSpeed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
}
