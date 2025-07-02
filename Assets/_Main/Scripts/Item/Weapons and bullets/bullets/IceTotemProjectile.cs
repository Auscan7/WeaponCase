using UnityEngine;

public class IceTotemProjectile : MonoBehaviour
{
    private float speed;
    private Vector2 direction;
    [SerializeField] private LayerMask enemyLayers;

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            if (collision.TryGetComponent(out CharacterStatManager enemyStats))
            {
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.iceTotemStats.damage);
            }
        }
    }
}
