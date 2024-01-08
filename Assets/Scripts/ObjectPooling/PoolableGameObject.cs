using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    public GameObject Prefab { get; set; }

    public void SetPrefab(GameObject prefab) => Prefab = prefab;
}
