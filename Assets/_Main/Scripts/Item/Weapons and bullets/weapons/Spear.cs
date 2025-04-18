using UnityEngine;
using System.Collections;

public class Spear : Weapon
{
    [Header("Spear Settings")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float defaultDamage = 15f;
    [SerializeField] private float defaultRange = 5f;
    [SerializeField] private float defaultFireRate = 1f;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private bool isThrown = false;
    private bool isReturning = false;
    private Rigidbody2D rb;
    private Collider2D spearCollider;
    private Transform parentTransform;
    private LineRenderer aimLine;
    private Vector2 throwDirection;
    private Vector2 worldThrowStartPosition;

    private void Awake()
    {
        // Store the parent transform for later use
        parentTransform = transform.parent;
        
        // Get and validate required components
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Spear requires a Rigidbody2D component!");
            enabled = false;
            return;
        }

        spearCollider = GetComponent<Collider2D>();
        if (spearCollider == null)
        {
            Debug.LogError("Spear requires a Collider2D component!");
            enabled = false;
            return;
        }
        spearCollider.isTrigger = true; // Ensure it's a trigger

        // Set up line renderer for aiming
        aimLine = GetComponent<LineRenderer>();
        if (aimLine == null)
        {
            aimLine = gameObject.AddComponent<LineRenderer>();
        }
        aimLine.positionCount = 2;
        aimLine.startWidth = 0.1f;
        aimLine.endWidth = 0.1f;
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = new Color(1f, 0f, 0f, 0.5f);
        aimLine.endColor = new Color(1f, 0f, 0f, 0.5f);
        aimLine.sortingLayerName = "Default";
        aimLine.sortingOrder = 210;
    }

    private void Start()
    {
        // Store initial local position and rotation
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        
        // Set weapon range from stats if available, otherwise use default
        if (PlayerUpgradeManager.Instance != null && PlayerUpgradeManager.Instance.spearStats != null)
        {
            weaponRange = PlayerUpgradeManager.Instance.spearStats.range;
        }
        else
        {
            weaponRange = defaultRange;
        }
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (!CanFire() || isThrown) return;

        // Get fire rate from stats if available, otherwise use default
        float fireRate = defaultFireRate;
        if (PlayerUpgradeManager.Instance != null && PlayerUpgradeManager.Instance.spearStats != null)
        {
            fireRate = PlayerUpgradeManager.Instance.spearStats.firerate;
        }

        // Only trigger cooldown if the manager exists
        if (WeaponCooldownUIManager.Instance != null)
        {
            WeaponCooldownUIManager.Instance.TriggerCooldown("Spear", fireRate);
        }

        // Only play sound if the audio manager exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.bowAndArrowFireSFX);
        }

        isThrown = true;
        worldThrowStartPosition = transform.position;
        throwDirection = (targetPosition - worldThrowStartPosition).normalized;
        
        // Detach from parent temporarily
        transform.SetParent(null);
        
        // Set velocity and rotation
        rb.linearVelocity = throwDirection * projectileSpeed;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(throwDirection.y, throwDirection.x) * Mathf.Rad2Deg);

        // Enable collision for damage
        spearCollider.enabled = true;

        SetNextFireTime(fireRate);
    }

    private void FixedUpdate()
    {
        if (isThrown)
        {
            if (!isReturning)
            {
                // Move in the throw direction
                rb.linearVelocity = throwDirection * projectileSpeed;

                // Check if we've reached max distance
                float distanceFromStart = Vector2.Distance(worldThrowStartPosition, transform.position);
                if (distanceFromStart >= weaponRange)
                {
                    StartReturning();
                }
            }
            else
            {
                // Calculate return direction to the player's current position
                Vector2 targetWorldPosition = parentTransform.TransformPoint(originalLocalPosition);
                Vector2 returnDirection = (targetWorldPosition - (Vector2)transform.position).normalized;
                rb.linearVelocity = returnDirection * projectileSpeed * 3f;

                // Check if we've returned close enough to the player
                if (Vector2.Distance(transform.position, targetWorldPosition) < 0.2f)
                {
                    ResetSpear();
                }
            }
        }
    }

    private void Update()
    {
        if (!isThrown)
        {
            // Update aiming line when not thrown
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spearPos = transform.position;
            
            aimLine.SetPosition(0, spearPos);
            aimLine.SetPosition(1, mousePos);
        }
        else
        {
            // Hide aiming line while thrown
            aimLine.SetPosition(0, Vector3.zero);
            aimLine.SetPosition(1, Vector3.zero);
        }
    }

    private void StartReturning()
    {
        isReturning = true;
        // Disable collision when returning to prevent damage
        spearCollider.enabled = false;
    }

    private void ResetSpear()
    {
        isThrown = false;
        isReturning = false;
        rb.linearVelocity = Vector2.zero;
        
        // Reattach to parent and reset local position/rotation
        transform.SetParent(parentTransform);
        transform.localPosition = originalLocalPosition;
        transform.localRotation = originalLocalRotation;
        
        spearCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isThrown || isReturning) return;

        // Check if we hit an enemy
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            // Get damage from stats if available, otherwise use default
            float damage = defaultDamage;
            if (PlayerUpgradeManager.Instance != null && PlayerUpgradeManager.Instance.spearStats != null)
            {
                damage = PlayerUpgradeManager.Instance.spearStats.damage;
            }

            // Apply damage to the enemy
            CharacterStatManager enemy = collision.GetComponentInParent<CharacterStatManager>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    public override void UpdateAimingLine(Vector2 targetPosition)
    {
        if (isThrown) return; // Don't update aiming line while thrown
        
        Vector2 spearPos = transform.position;
        
        aimLine.SetPosition(0, spearPos);
        aimLine.SetPosition(1, targetPosition);
    }

    public override void HideAimingLine()
    {
        if (aimLine == null) return;
        aimLine.SetPosition(0, Vector3.zero);
        aimLine.SetPosition(1, Vector3.zero);
    }

    public override void RotateTowardsTarget(Vector2 targetPosition)
    {
        // Only rotate if the spear is not thrown
        if (!isThrown)
        {
            base.RotateTowardsTarget(targetPosition);
        }
    }
}
