using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : CharacterMovement
{
    PlayerManager player;

    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float verticalMovement;

    [Header("Movement Settings")]
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float movementSpeedMultiplier = 1;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
    }

    private void GetMovementValues()
    {
        horizontalMovement = PlayerInputManager.instance.horizontal_Input;
        verticalMovement = PlayerInputManager.instance.vertical_Input;
    }

    private void HandleMovement()
    {
        GetMovementValues();

        // Combine horizontal and vertical inputs
        Vector2 inputVector = new Vector2(horizontalMovement, verticalMovement);

        // Normalize to prevent diagonal speed boost
        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }

        // Calculate speed multiplier based on moveAmount
        float speedMultiplier = Mathf.Abs(PlayerInputManager.instance.moveAmount);
        float effectiveSpeed = (walkingSpeed * movementSpeedMultiplier) * (speedMultiplier / 1.5f);

        // Apply movement
        player.rb.velocity = new Vector2(inputVector.x * effectiveSpeed, inputVector.y * effectiveSpeed);
    }
}