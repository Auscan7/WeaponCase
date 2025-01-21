using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownFishEnemyStatManager : CharacterStatManager
{
    GemDrop gemDrop;

    protected override void Awake()
    {
        base.Awake();
        gemDrop = GetComponent<GemDrop>();
    }
    protected override void HandleDeath()
    {
        base.HandleDeath();
        gemDrop.DropGem();
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
