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

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (hammerheadCombatManager != null)
        {
            hammerheadCombatManager.TryConeAttack(player);
        }
    }
}
