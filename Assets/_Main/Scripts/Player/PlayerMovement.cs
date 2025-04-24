using UnityEngine;

public class PlayerMovement : CharacterMovementManager
{
    PlayerManager player;

    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float verticalMovement;

    [Header("Movement Settings")]
    private float sailingSpeed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
        sailingSpeed = PlayerUpgradeManager.Instance.playerSailingSpeed;
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
        float effectiveSpeed = (sailingSpeed * PlayerUpgradeManager.Instance.playerMovementSpeedMultiplier) * (speedMultiplier / 1.5f);

        // Apply movement
        player.rb.linearVelocity = new Vector2(inputVector.x * effectiveSpeed, inputVector.y * effectiveSpeed);

        // Rotate player towards movement direction if moving
        if (inputVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}