using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownFishEnemyStatManager : CharacterStatManager
{
    GemDrop gemDrop;
    EnemyItemDrop itemDrop;
    private float damageTimer = 0f;
    private float hideHealthBarDelay = 3f;

    protected override void Awake()
    {
        base.Awake();
        gemDrop = GetComponent<GemDrop>();
        itemDrop = GetComponent<EnemyItemDrop>();
    }

    public override void HandleDeath()
    {
        base.HandleDeath();
        gemDrop.DropGem();
        itemDrop.DropItem();
    }

    protected override void Update()
    {
        base.Update();
        if(healthBarParent == null)
            return;

        if (currentHealth == maxHealth)
        {
            healthBarParent.SetActive(false);
        }
        else
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= hideHealthBarDelay)
            {
                healthBarParent.SetActive(false);
            }
            else
            {
                healthBarParent.SetActive(true);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        damageTimer = 0f;
    }
}
