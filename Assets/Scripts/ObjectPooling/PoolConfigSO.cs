using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolConfig", menuName = "Pooling/Pool Config", order = 1)]
public class PoolConfigSO : ScriptableObject
{
    public GameObject prefab;
    public int initialSize;

    // Spawn Rate Settings
    public float baseSpawnRate; // Base spawn rate
    public float maxSpawnRate; // Maximum spawn rate

    // Score Threshold Settings
    public int scoreForBaseRate; // Score at which this object starts spawning
    public int scoreForMaxRate; // Score at which max spawn rate is reached

    // Special Object Settings
    public bool isSpecialObject; // Identifies if the object is a special case (e.g., powerup)
    public float specialSpawnTimer; // Special spawn rate for this object
}