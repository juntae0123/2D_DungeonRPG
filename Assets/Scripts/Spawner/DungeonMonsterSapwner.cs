using UnityEngine;
using System.Collections.Generic;

public class DungeonMonsterSpawner : MonoBehaviour
{
    public GameObject warriorPrefab;
    public GameObject bowSkeletonPrefab;
    public GameObject semiBossPrefab;

    public int warriorCount = 10;
    public int bowSkeletonCount = 10;

    private float mapWidth = 48.54f;
    private float mapHeight = 16.03f;
    private float edgeBuffer = 0.5f;

    private List<GameObject> monsters = new List<GameObject>();

    void Start()
    {
        // 모든 적을 한꺼번에 스폰
        SpawnSemiBoss();
        SpawnWarriorsAndBows();
        Debug.Log("All monsters spawned at once. Initial monsters count: " + monsters.Count);
    }

    void SpawnWarriorsAndBows()
    {
        for (int i = 0; i < warriorCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject warrior = Instantiate(warriorPrefab, spawnPosition, Quaternion.identity);
            warrior.tag = "Enemy";
            monsters.Add(warrior);

            EnemyBase enemyBase = warrior.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                EnemyBase.OnDeath += () => OnMonsterDeath(warrior);
                Debug.Log("OnDeath event connected for warrior.");
            }
        }

        for (int i = 0; i < bowSkeletonCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject bowSkeleton = Instantiate(bowSkeletonPrefab, spawnPosition, Quaternion.identity);
            bowSkeleton.tag = "Enemy";
            monsters.Add(bowSkeleton);

            EnemyBase enemyBase = bowSkeleton.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                EnemyBase.OnDeath += () => OnMonsterDeath(bowSkeleton);
                Debug.Log("OnDeath event connected for bow skeleton.");
            }
        }
    }

    void SpawnSemiBoss()
    {
        if (semiBossPrefab != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject semiBoss = Instantiate(semiBossPrefab, spawnPosition, Quaternion.identity);
            semiBoss.tag = "Enemy";
            monsters.Add(semiBoss);
            Debug.Log("SemiBoss spawned at position: " + spawnPosition);
        }
        else
        {
            Debug.LogError("SemiBoss prefab is not assigned!");
        }
    }

    void OnMonsterDeath(GameObject monster)
    {
        if (monsters.Contains(monster))
        {
            monsters.Remove(monster);
            Debug.Log("Monster removed, remaining: " + monsters.Count);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = 0f;
        float y = 0f;

        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: // 상단
                x = Random.Range(-mapWidth / 2f + edgeBuffer, mapWidth / 2f - edgeBuffer);
                y = mapHeight / 2f - edgeBuffer;
                break;
            case 1: // 하단
                x = Random.Range(-mapWidth / 2f + edgeBuffer, mapWidth / 2f - edgeBuffer);
                y = -mapHeight / 2f + edgeBuffer;
                break;
            case 2: // 좌측
                x = -mapWidth / 2f + edgeBuffer;
                y = Random.Range(-mapHeight / 2f + edgeBuffer, mapHeight / 2f - edgeBuffer);
                break;
            case 3: // 우측
                x = mapWidth / 2f - edgeBuffer;
                y = Random.Range(-mapHeight / 2f + edgeBuffer, mapHeight / 2f - edgeBuffer);
                break;
        }

        return new Vector3(x, y, 0);
    }
}