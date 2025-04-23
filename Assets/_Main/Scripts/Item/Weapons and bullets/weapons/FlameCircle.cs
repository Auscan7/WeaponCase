using UnityEngine;
using System.Collections;

public class FlameCircle : Weapon
{
    [Header("Flame Circle Settings")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private ParticleSystem outerRingParticles;
    
    private float damageInterval;
    private float nextDamageTime;
    private bool isActive = false;
    private float originalRadius;
    private float originalOuterRadius;
    private Transform particleContainer;
    private Vector3 lastPosition;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.Particle[] outerParticles;

    protected override void Awake()
    {
        base.Awake();
        InitializeParticleSystems();
    }

    private void InitializeParticleSystems()
    {
        // Create a container for particles that won't rotate
        particleContainer = new GameObject("ParticleContainer").transform;
        particleContainer.SetParent(null);
        particleContainer.position = transform.position;
        lastPosition = transform.position;
        
        if (flameParticles != null)
        {
            var shape = flameParticles.shape;
            originalRadius = shape.radius;
            
            flameParticles.transform.SetParent(particleContainer);
            
            var main = flameParticles.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.playOnAwake = true;
            main.maxParticles = 1000;
            
            particles = new ParticleSystem.Particle[main.maxParticles];
        }
        
        if (outerRingParticles != null)
        {
            var shape = outerRingParticles.shape;
            originalOuterRadius = shape.radius;
            
            outerRingParticles.transform.SetParent(particleContainer);
            
            var main = outerRingParticles.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.playOnAwake = true;
            main.maxParticles = 1000;
            
            outerParticles = new ParticleSystem.Particle[main.maxParticles];
        }
        
        UpdateStats();
    }

    private void Start()
    {
        StartCoroutine(DamageRoutine());
    }

    private void UpdateStats()
    {
        // Update range and damage interval from PlayerUpgradeManager
        weaponRange = PlayerUpgradeManager.Instance.flameCircleStats.range;
        damageInterval = 1f / PlayerUpgradeManager.Instance.flameCircleStats.firerate;
        
        // Update particle systems
        UpdateParticleSystemScale();
    }

    private void UpdateParticleSystemScale()
    {
        if (flameParticles != null)
        {
            var shape = flameParticles.shape;
            // Only update the radius, don't scale particle size or emission rate
            shape.radius = weaponRange;
        }

        if (outerRingParticles != null)
        {
            var shape = outerRingParticles.shape;
            // Only update the radius, don't scale particle size or emission rate
            shape.radius = weaponRange;
        }
    }

    private void FixedUpdate()
    {
        UpdateStats();
        
        if (particleContainer != null)
        {
            Vector3 currentPosition = transform.position;
            Vector3 positionDelta = currentPosition - lastPosition;
            
            particleContainer.position += positionDelta;
            
            if (flameParticles != null)
            {
                int numParticles = flameParticles.GetParticles(particles);
                for (int i = 0; i < numParticles; i++)
                {
                    particles[i].position += positionDelta;
                }
                flameParticles.SetParticles(particles, numParticles);
            }
            
            if (outerRingParticles != null)
            {
                int numOuterParticles = outerRingParticles.GetParticles(outerParticles);
                for (int i = 0; i < numOuterParticles; i++)
                {
                    outerParticles[i].position += positionDelta;
                }
                outerRingParticles.SetParticles(outerParticles, numOuterParticles);
            }
            
            lastPosition = currentPosition;
        }
    }

    private IEnumerator DamageRoutine()
    {
        isActive = true;
        while (isActive)
        {
            if (Time.time >= nextDamageTime)
            {
                DealDamageToEnemiesInRange();
                nextDamageTime = Time.time + damageInterval;
            }
            yield return null;
        }
    }

    private void DealDamageToEnemiesInRange()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out CharacterStatManager enemyStats))
            {
                enemyStats.TakeDamage(PlayerUpgradeManager.Instance.flameCircleStats.damage, Color.red);
                WeaponCooldownUIManager.Instance.TriggerCooldown("FlameCircle", PlayerUpgradeManager.Instance.flameCircleStats.firerate);
            }
        }
    }

    public override void Fire(Vector2 targetPosition)
    {
        // Flame Circle doesn't need to fire, it's always active
    }

    private void OnDestroy()
    {
        isActive = false;
        if (particleContainer != null)
        {
            Destroy(particleContainer.gameObject);
        }
    }

    // Draw the damage radius in the editor for easier visualization
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, weaponRange);
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, weaponRange);
    }
}

