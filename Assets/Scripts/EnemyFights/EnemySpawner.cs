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
    [SerializeField] private int spawnCooldown = 5;

    private Dictionary<Transform, float> lastSpawnTimes = new Dictionary<Transform, float>();


    private bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<Coroutine> spawnCoroutines = new List<Coroutine>();


    public void Awake()
    {
        foreach (Transform point in spawnPoints)
        {
            lastSpawnTimes[point] = -spawnCooldown;
        }
    }
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

        
    }

    private IEnumerator SpawnEnemyAfterRandomDelay()
    {
        float randomDelay = Random.Range(0f, spawnWindow);
        yield return new WaitForSeconds(randomDelay);

        if (!isSpawning) yield break;

        List<Transform> availableSpawnPoints = new List<Transform>();

        while (availableSpawnPoints.Count == 0)
        {
            availableSpawnPoints.Clear();

            foreach (Transform point in spawnPoints)
            {
                if (Time.time - lastSpawnTimes[point] >= spawnCooldown)
                {
                    availableSpawnPoints.Add(point);
                }
            }

            if (availableSpawnPoints.Count == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }


        Transform spawnPoint =
            availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

        lastSpawnTimes[spawnPoint] = Time.time;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        spawnedEnemies.Add(newEnemy);
        AudioManager.instance.Play("GhostSpawn");

        Debug.Log("Ghost spawned.");
    }
}