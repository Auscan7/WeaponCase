using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [SerializeField] protected float damageAmount = 5f;
    CharacterManager characterManager;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Attack(GameObject target)
    {
        CharacterStatManager targetStats = target.GetComponentInParent<CharacterStatManager>();

        if (targetStats != null)
        {
            targetStats.TakeDamage(damageAmount);
        }
    }

    protected virtual void Update()
    {
        
    }
}
