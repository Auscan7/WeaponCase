using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerMovement playerMovement;

    protected override void Awake()
    {
        base.Awake();

        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void Update()
    {
        base.Update();

        playerMovement.HandleAllMovement();
    }
}
