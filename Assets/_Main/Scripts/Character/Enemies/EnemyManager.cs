using UnityEngine;

public class EnemyManager : CharacterManager
{
    private EnemyCombatManager enemyCombatManager;
    private HammerheadCombat hammerheadCombatManager;
    private LanternFishCombat lanternFishCombatManager;
    public Transform player { get; private set; } // Public getter, private setter

    protected override void Awake()
    {
        base.Awake();
        enemyCombatManager = GetComponent<EnemyCombatManager>();
        hammerheadCombatManager = GetComponent<HammerheadCombat>();
        lanternFishCombatManager = GetComponent<LanternFishCombat>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        base.Update();
        if (player == null) return;

        if (hammerheadCombatManager != null)
        {
            hammerheadCombatManager.TryConeAttack(player);
        }
        else if (lanternFishCombatManager != null)
        {
            lanternFishCombatManager.TryProjectileAttack(player);
        }
    }
}
