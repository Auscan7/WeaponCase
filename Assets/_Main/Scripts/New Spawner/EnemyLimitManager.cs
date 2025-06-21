using UnityEngine;

public class EnemyLimitManager : MonoBehaviour
{
    public static EnemyLimitManager Instance;

    public int maxActiveEnemies = 100;
    private int activeEnemyCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanSpawnEnemy(bool ignoreLimit = false)
    {
        return ignoreLimit || activeEnemyCount < maxActiveEnemies;
    }

    public void RegisterEnemy()
    {
        activeEnemyCount++;
    }

    public void UnregisterEnemy()
    {
        activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
    }

    public int GetActiveEnemyCount()
    {
        return activeEnemyCount;
    }
}
