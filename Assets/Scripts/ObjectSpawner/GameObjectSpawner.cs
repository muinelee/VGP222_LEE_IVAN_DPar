using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<PoolConfigSO> poolConfigs;
    private Dictionary<GameObject, float> nextSpawnTimes = new Dictionary<GameObject, float>();

    public PoolManager pm;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
        InitializeSpawnTimers();
    }

    private void InitializeSpawnTimers()
    {
        foreach (var config in poolConfigs)
        {
            nextSpawnTimes[config.prefab] = Time.time + (config.isSpecialObject ? config.specialSpawnTimer : GetSpawnRate(config));
        }
    }

    void Update()
    {
        foreach (var config in poolConfigs)
        {
            if (Time.time >= nextSpawnTimes[config.prefab])
            {
                if (ShouldSpawn(config))
                {
                    SpawnObject(config.prefab);
                    nextSpawnTimes[config.prefab] = Time.time + (config.isSpecialObject ? config.specialSpawnTimer : GetSpawnRate(config));
                }
            }
        }
    }

    private bool ShouldSpawn(PoolConfigSO config)
    {
        return GameManager.Instance.Score >= config.scoreForBaseRate;
    }

    private void SpawnObject(GameObject prefab)
    {
        var objectToSpawn = pm.GetObject(prefab);
        objectToSpawn.transform.position = GetSpawnPosition();
    }

    private float GetSpawnRate(PoolConfigSO config)
    {
        var playerScore = GameManager.Instance.Score;
        return Mathf.Max(0.5f, Mathf.Lerp(config.baseSpawnRate, config.maxSpawnRate, playerScore / (float)config.scoreForMaxRate));
    }

    private Vector3 GetSpawnPosition()
    {
        var halfWidth = transform.localScale.x / 2;
        var randomX = Random.Range(-halfWidth, halfWidth);
        return transform.position + new Vector3(randomX, 0f, 0f);
    }
}