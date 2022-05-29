using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Timer))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    [SerializeField] private int maxSimultaneousSpawns;
    [SerializeField] private int minSimultaneousSpawns;
    [SerializeField] private Vector2 spawnArea;
    [SerializeField] private Color[] colors;
    [SerializeField] private Timer spawnTimer;

    private void Update()
    {
        if (EnemyPool.ActiveEnemies < EnemyPool.MaxActiveEnemies && spawnTimer.IsCompleted)
        {
            var spawns = Random.Range(minSimultaneousSpawns, maxSimultaneousSpawns);
            for (int i = 0; i < spawns; i++)
            {
                var spawnPosition = new Vector3(transform.position.x + Random.Range(0, spawnArea.x),
                    transform.position.y + Random.Range(0, spawnArea.y), 0);
                EnemyPool.Spawn(spawnPosition, colors[Random.Range(0, colors.Length - 1)]);
                spawnTimer.SetTime(spawnCooldown);
                spawnTimer.StartTimer();
            }
        }
    }

    private void Start()
    {
        spawnTimer.SetTime(spawnCooldown);
        spawnTimer.StartTimer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(spawnArea.x, spawnArea.y) / 2, new Vector3(spawnArea.x, spawnArea.y));
    }
}
