using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Attack(collision.gameObject);
        }
    }
}
