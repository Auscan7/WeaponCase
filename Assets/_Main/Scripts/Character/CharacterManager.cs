using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CharacterMovementManager characterMovementManager;
    [HideInInspector] public CharacterStatManager characterStatManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;   

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterMovementManager = GetComponent<CharacterMovementManager>();
        characterStatManager = GetComponent<CharacterStatManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
    }

    protected virtual void Update()
    {

    }
}
