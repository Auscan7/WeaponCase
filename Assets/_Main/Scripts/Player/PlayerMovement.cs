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

}