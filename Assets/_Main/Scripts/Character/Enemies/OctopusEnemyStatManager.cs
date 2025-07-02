using UnityEngine;

public class OctopusEnemyStatManager : EnemyStatManager
{
    public override void HandleDeath()
    {
        base.HandleDeath();

        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.OnBossDefeated();
        }
        else
        {
            Debug.LogError("GameTimeManager instance not found!");
        }
    }
}
