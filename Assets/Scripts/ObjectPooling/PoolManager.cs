using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolConfigSO> poolConfigs;
    private Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {
            pools[config.prefab] = new GameObjectPool(config.prefab, config.initialSize);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            Debug.LogWarning($"Creating a default pool for {prefab.name} as it was not initialized.");
            pool = new GameObjectPool(prefab, 10);
            pools[prefab] = pool;
        }

        return pool.GetObject();
    }

    public void ReturnObject(GameObject gameObject, PoolableGameObject poolable)
    {
        if (pools.TryGetValue(poolable.Prefab, out var pool))
        {
            pool.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogWarning($"Returned object {gameObject.name} has no matching pool.");
            Destroy(gameObject);
        }
    }
}