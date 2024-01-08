using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign in the inspector
    public Transform gunSpawnPoint; // Assign in the inspector
    public float shootingRate = 2.0f;
    public PoolManager poolManager; // Assign in the inspector or via script

    private float nextShotTime;

    private void Start()
    {
        if (poolManager == null)
        {
            poolManager = FindObjectOfType<PoolManager>(); // It's better to assign this via the editor if possible
        }
        nextShotTime = Time.time + shootingRate;
    }

    private void Update()
    {
        if (Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + shootingRate;
        }
    }

    private void Shoot()
    {
        GameObject projectile = poolManager.GetObject(projectilePrefab); // Get a projectile from the pool
        if (projectile != null)
        {
            projectile.transform.position = gunSpawnPoint.position;
            projectile.transform.rotation = Quaternion.identity; // Or your desired rotation
            projectile.SetActive(true);
        }
    }
}