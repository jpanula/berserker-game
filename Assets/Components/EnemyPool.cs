using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private int poolSize;
    [SerializeField] private EnemyUnit enemyPrefab;
    
    private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();


    public static int MaxActiveEnemies
    {
        get { return Instance.poolSize; }
    }
    
    public static int ActiveEnemies
    {
        get
        {
            int numActiveEnemies = 0;
            foreach (var enemyUnit in Instance._enemyUnits)
            {
                if (enemyUnit.gameObject.activeSelf)
                {
                    numActiveEnemies++;
                }
            }

            return numActiveEnemies;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var enemy = Instantiate(enemyPrefab, transform);
            enemy.gameObject.SetActive(false);
            _enemyUnits.Add(enemy.GetComponent<EnemyUnit>());
        }
    }

    public static bool Spawn(Vector3 position, Color color)
    {
        foreach (var enemy in Instance._enemyUnits)
        {
            if (!enemy.gameObject.activeSelf)
            {
                enemy.transform.position = position;
                enemy.Color = color;
                enemy.gameObject.SetActive(true);
                return true;
            }
        }

        return false;
    }
}
