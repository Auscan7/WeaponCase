using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [SerializeField] protected float damageAmount = 5f;

    public virtual void Attack(GameObject target)
    {
        CharacterStatManager targetStats = target.GetComponentInParent<CharacterStatManager>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(damageAmount);
        }
    }
}
