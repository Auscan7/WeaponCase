using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public abstract class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed;
    public Sprite weaponIcon;
    [HideInInspector]public float weaponRange; // Each weapon defines its own range

    protected float nextFireTime = 0f;

    [Header("Weapon Settings")]
    public bool isManualWeapon = false;

    private LineRenderer aimLine;

    protected virtual void Awake()
    {
        if (isManualWeapon)
        {
            aimLine = GetComponent<LineRenderer>();
            if (aimLine == null)
            {
                aimLine = gameObject.AddComponent<LineRenderer>();
            }

            // Configure basic line style (you can adjust this in the Inspector if you want)
            aimLine.positionCount = 2;
            aimLine.startWidth = 0.055f;
            aimLine.endWidth = 0.055f;

            aimLine.material = new Material(Shader.Find("Sprites/Default"));

            Color transparentRed = new Color(1f, 0f, 0f, 0.35f); // RGBA
            aimLine.startColor = transparentRed;
            aimLine.endColor = transparentRed;

            aimLine.sortingLayerName = "Default"; // or "Player", etc.
            aimLine.sortingOrder = 210; // higher = draws in front
        }
    }

    public virtual void UpdateAimingLine(Vector2 targetPosition)
    {
        if (!isManualWeapon || aimLine == null) return;

        Vector3 start = projectileSpawnPoint.position;
        Vector3 end = targetPosition;

        start.z = 0f;
        end.z = 0f;
        aimLine.SetPosition(0, start);
        aimLine.SetPosition(1, end);

    }

    public virtual void HideAimingLine()
    {
        if (aimLine != null)
        {
            aimLine.SetPosition(0, Vector3.zero);
            aimLine.SetPosition(1, Vector3.zero);
        }
    }

    public abstract void Fire(Vector2 targetPosition);

    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void SetNextFireTime(float fireRate)
    {
        nextFireTime = Time.time + (1f / fireRate);
    }

    public virtual void RotateTowardsTarget(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
