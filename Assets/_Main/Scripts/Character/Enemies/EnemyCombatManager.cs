using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    EnemyManager enemyManager;
    HammerheadCombat hammerheadCombatManager;

    override protected void Awake()
    {
        base.Awake();
        hammerheadCombatManager = GetComponent<HammerheadCombat>();
    }

    override protected void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        float multiplier = DifficultyManager.instance.GetCurrentEnemyDamageMultiplier();
        damageAmount *= multiplier;
    }

    protected override void Update()
    {
        if (enemyManager.player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, enemyManager.player.position);

        if (hammerheadCombatManager != null)
        {
            hammerheadCombatManager.TryConeAttack(enemyManager.player);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Attack(collision.gameObject);
        }
    }
}
