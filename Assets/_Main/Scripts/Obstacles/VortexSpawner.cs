using UnityEngine;
using System.Collections;

public class VortexSpawner : MonoBehaviour
{
    [SerializeField] private GameObject vortexPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float vortexSpeed = 5f;
    [SerializeField] private float minSpawnTime = 30f;
    [SerializeField] private float maxSpawnTime = 60f;

    private bool isPaused = false;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        spawnCoroutine = StartCoroutine(VortexSpawnRoutine());
    }

    private IEnumerator VortexSpawnRoutine()
    {
        while (true)
        {
            float randomDelay = Random.Range(minSpawnTime, maxSpawnTime);
            float timer = 0f;

            while (timer < randomDelay)
            {
                if (!isPaused)
                    timer += Time.deltaTime;

                yield return null;
            }

            if (!isPaused)
                SpawnVortex();
        }
    }

    public void PauseSpawning()
    {
        isPaused = true;
    }

    public void ResumeSpawning()
    {
        isPaused = false;
    }

    public void SpawnVortex()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not set!");
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = player.position + (Vector3)(randomDirection * spawnDistance);

        GameObject vortex = Instantiate(vortexPrefab, spawnPosition, vortexPrefab.transform.rotation);

        Vector3 routeDirection = (player.position - spawnPosition).normalized;
        VortexMovement vortexMovement = vortex.GetComponent<VortexMovement>();

        if (vortexMovement != null)
            vortexMovement.Initialize(routeDirection, vortexSpeed);
        else
            Debug.LogError("Vortex prefab is missing the VortexMovement component.");
    }
}
