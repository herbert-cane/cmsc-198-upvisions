using System.Collections;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [Tooltip("Prefab for the background piece to spawn")]
    [SerializeField] private GameObject backgroundPrefab;

    [Tooltip("Seconds between spawns")]
    [SerializeField] private float spawnInterval = 3f;

    [Tooltip("Local offset from the spawner position where the prefab will be instantiated")]
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Spawn one immediately on Start")]
    [SerializeField] private bool spawnOnStart = true;

    private Coroutine spawnRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundPrefab == null)
        {
            Debug.LogWarning($"{nameof(BackgroundSpawner)}: backgroundPrefab not assigned on '{gameObject.name}'. Disabling spawner.");
            enabled = false;
            return;
        }

        if (spawnOnStart)
            SpawnOnce();

        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnOnce();
        }
    }

    private void SpawnOnce()
    {
        Instantiate(backgroundPrefab, transform.position + spawnOffset, Quaternion.identity);
    }

    void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }
}
