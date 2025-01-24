using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CharacterMovement characterMovement;
    [HideInInspector] public CharacterStatManager characterStatManager;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterMovement = GetComponent<CharacterMovement>();
        characterStatManager = GetComponent<CharacterStatManager>();
    }

    protected virtual void Update()
    {

    }
}
