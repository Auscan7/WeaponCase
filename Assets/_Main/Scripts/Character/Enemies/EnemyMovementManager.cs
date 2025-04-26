using UnityEngine;

public class EnemyMovementManager : CharacterMovementManager
{
    private EnemyManager enemyManager;
    protected Transform player;
    public float moveSpeed = 5f;
    public Transform spriteTransform;
    public float rotationSpeed = 5f;
    [SerializeField]private float stoppingDistance = 1f;

    public bool isAttacking { get; set; } // This flag will be controlled by HammerheadCombat

    protected override void Awake()
    {
        base.Awake();
        enemyManager = GetComponent<EnemyManager>();
    }

    protected override void Start()
    {
        base.Start();

        if (enemyManager != null)
        {
            player = enemyManager.player;
        }
    }

    protected override void Update()
    {
        if (player == null || isAttacking) return;

        Vector2 direction = (player.position - transform.position);
        if (direction.magnitude > stoppingDistance)
        {
            Move(direction.normalized, moveSpeed);
            RotateTowards(direction.normalized);
        }
    }


    public void RotateTowards(Vector2 direction)
    {
        if (spriteTransform != null && direction != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(spriteTransform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            spriteTransform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
        }
    }
}
