using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Timing")]
    [SerializeField] private int enemiesToSpawn = 4;
    [SerializeField] private float spawnWindow = 20f;

    private bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<Coroutine> spawnCoroutines = new List<Coroutine>();

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        spawnedEnemies.Clear();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Coroutine c = StartCoroutine(SpawnEnemyAfterRandomDelay());
            spawnCoroutines.Add(c);
        }

        Debug.Log("Ritual spawning started.");
    }

    public void StopAndResetSpawning()
    {
        if (!isSpawning) return;

        isSpawning = false;

        foreach (Coroutine c in spawnCoroutines)
        {
            if (c != null)
            {
                StopCoroutine(c);
            }
        }

        spawnCoroutines.Clear();

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear();

        Debug.Log("Ritual failed. Ghosts vanished.");
    }

    private IEnumerator SpawnEnemyAfterRandomDelay()
    {
        float randomDelay = Random.Range(0f, spawnWindow);
        yield return new WaitForSeconds(randomDelay);

        if (!isSpawning) yield break;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        spawnedEnemies.Add(newEnemy);

        Debug.Log("Ghost spawned.");
    }
}