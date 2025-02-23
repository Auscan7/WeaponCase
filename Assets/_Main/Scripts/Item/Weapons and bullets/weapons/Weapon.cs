using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed;
    [HideInInspector]public float weaponRange; // Each weapon defines its own range

    protected float nextFireTime = 0f;

    public abstract void Fire(Vector2 targetPosition);

    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void SetNextFireTime(float fireRate)
    {
        nextFireTime = Time.time + (1f / fireRate);
    }
}
