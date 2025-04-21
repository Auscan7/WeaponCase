using UnityEngine;
using System.Collections;

public class FlameCircle : Weapon
{
    [Header("Flame Circle Settings")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private Material flameMaterial;
    
    private float damageInterval;
    private float nextDamageTime;
    private bool isActive = false;

    protected override void Awake()
    {
        base.Awake();
        weaponRange = PlayerUpgradeManager.Instance.flameCircleStats.range;
        damageInterval = 1f / PlayerUpgradeManager.Instance.flameCircleStats.firerate;
        InitializeFlameEffect();
    }

    private void InitializeFlameEffect()
    {
        // Create or get ParticleSystem
        if (flameParticles == null)
        {
            GameObject particlesObj = new GameObject("FlameParticles");
            particlesObj.transform.SetParent(transform);
            particlesObj.transform.localPosition = Vector3.zero;
            flameParticles = particlesObj.AddComponent<ParticleSystem>();
        }

        // Get or create renderer
        var renderer = flameParticles.GetComponent<ParticleSystemRenderer>();
        if (renderer == null)
        {
            renderer = flameParticles.gameObject.AddComponent<ParticleSystemRenderer>();
        }

        // Set up the material
        if (flameMaterial == null)
        {
            // Create a new material with the default particle shader
            flameMaterial = new Material(Shader.Find("Particles/Additive"));
            // Set the texture to a simple white texture
            Texture2D whiteTexture = new Texture2D(1, 1);
            whiteTexture.SetPixel(0, 0, Color.white);
            whiteTexture.Apply();
            flameMaterial.mainTexture = whiteTexture;
        }
        renderer.material = flameMaterial;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        renderer.sortMode = ParticleSystemSortMode.Distance;
        renderer.sortingOrder = 1;

        // Configure ParticleSystem
        var main = flameParticles.main;
        main.startLifetime = 1f;
        main.startSpeed = 0.5f;
        main.startSize = 0.5f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.loop = true;
        main.playOnAwake = true;
        main.maxParticles = 2000;
        main.scalingMode = ParticleSystemScalingMode.Local;

        var emission = flameParticles.emission;
        emission.rateOverTime = 500f;

        var shape = flameParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = weaponRange;
        shape.radiusThickness = 0.2f;
        shape.arc = 360f;
        shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
        shape.arcSpread = 0f;
        shape.arcSpeed = 1f;

        var velocity = flameParticles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.World;
        velocity.radial = new ParticleSystem.MinMaxCurve(1f);

        var color = flameParticles.colorOverLifetime;
        color.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(new Color(1f, 0.2f, 0f), 0.0f),
                new GradientColorKey(new Color(1f, 0.5f, 0f), 0.3f),
                new GradientColorKey(new Color(1f, 0.8f, 0f), 0.7f),
                new GradientColorKey(new Color(1f, 0.9f, 0.3f), 1.0f)
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.9f, 0.3f),
                new GradientAlphaKey(0.7f, 0.7f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        color.color = gradient;

        var size = flameParticles.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.3f),
            new Keyframe(0.3f, 0.5f),
            new Keyframe(0.7f, 0.3f),
            new Keyframe(1f, 0f)
        ));

        var noise = flameParticles.noise;
        noise.enabled = true;
        noise.strength = 0.2f;
        noise.frequency = 0.5f;
        noise.scrollSpeed = 0.5f;

        flameParticles.Play();
    }

    private void Start()
    {
        StartCoroutine(DamageRoutine());
    }

    private void Update()
    {
        if (flameParticles != null)
        {
            var shape = flameParticles.shape;
            shape.radius = weaponRange;
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
        if (flameParticles != null)
        {
            Destroy(flameParticles.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, weaponRange);
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, weaponRange);
    }
}

