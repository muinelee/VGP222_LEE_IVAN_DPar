using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    public GameObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;
        ExpandPool(initialSize);
    }

    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObject = GameObject.Instantiate(prefab);
            newObject.SetActive(false);
            var poolable = newObject.GetComponent<PoolableGameObject>() ?? newObject.AddComponent<PoolableGameObject>();
            poolable.SetPrefab(prefab);
            objectPool.Enqueue(newObject);
        }
    }

    public GameObject GetObject()
    {
        if (objectPool.Count == 0)
        {
            ExpandPool(1);
        }

        var obj = objectPool.Dequeue();
        obj.SetActive(true);
        ResetObject(obj);
        return obj;
    }

    public void ReturnObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        ResetObject(gameObject);
        objectPool.Enqueue(gameObject);
    }

    private void ResetObject(GameObject gameObject)
    {
        var resettable = gameObject.GetComponent<IResettable>();
        resettable?.ResetObject();
    }
}