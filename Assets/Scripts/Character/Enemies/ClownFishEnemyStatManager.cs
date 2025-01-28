using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownFishEnemyStatManager : CharacterStatManager
{
    GemDrop gemDrop;
    EnemyDropHealth healthDrop;

    protected override void Awake()
    {
        base.Awake();
        gemDrop = GetComponent<GemDrop>();
        healthDrop = GetComponent<EnemyDropHealth>();
    }
    public override void HandleDeath()
    {
        base.HandleDeath();
        gemDrop.DropGem();
        healthDrop.DropHealthItem();
    }

    protected override void Update()
    {
        base.Update();

        if (currentHealth == maxHealth)
        {
            healthBarParent.SetActive(false);
        }
        else
        {
            healthBarParent.SetActive(true);
        }
    }
}
