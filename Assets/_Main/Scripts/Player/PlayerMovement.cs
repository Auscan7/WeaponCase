using UnityEngine;

public class PlayerMovement : CharacterMovementManager
{
    PlayerManager player;

    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float verticalMovement;

    [Header("Movement Settings")]
    private float sailingSpeed;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 720f; // degrees per second

    [Header("Ripple Settings")]
    [SerializeField] private ParticleSystem rippleParticleSystem; // The ripple particle system as a child of the player

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
        sailingSpeed = PlayerUpgradeManager.Instance.playerSailingSpeed;

        // Ensure the particle system is stopped at the start
        if (rippleParticleSystem != null)
        {
            rippleParticleSystem.Stop();
        }
    }

    protected override void Update()
    {
        base.Update();

        // Check if the boat is moving and if the timer exceeds the spawn rate
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            PlayRipple();
        }
        else if (horizontalMovement == 0 && verticalMovement == 0)
        {
            // Stop the particle effect if the boat is not moving
            StopRipple();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleAllMovement();
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

        Vector2 inputVector = new Vector2(horizontalMovement, verticalMovement);

        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }

        float effectiveSpeed = sailingSpeed * PlayerUpgradeManager.Instance.playerMovementSpeedMultiplier;

        player.rb.linearVelocity = inputVector * effectiveSpeed;

        if (inputVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            player.transform.rotation = Quaternion.RotateTowards(
                player.transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    // Function to play the ripple particle effect
    private void PlayRipple()
    {
        if (rippleParticleSystem != null && !rippleParticleSystem.isPlaying)
        {
            rippleParticleSystem.Play();
        }
    }

    // Function to stop the ripple particle effect
    private void StopRipple()
    {
        if (rippleParticleSystem != null && rippleParticleSystem.isPlaying)
        {
            rippleParticleSystem.Stop();
        }
    }
}
