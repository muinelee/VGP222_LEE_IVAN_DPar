using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Guns
{
    public GameObject lGun, cGun, rGun;
}

public class PlayerShoot : MonoBehaviour
{
    [Header("General Projectile Stats")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [HideInInspector] public float nextFire;

    [Range(1, 3)]
    [SerializeField] private int weaponPower = 1;
    [HideInInspector] private int maxWeaponPower = 3;

    public Guns guns;
    bool shootingIsActive = true;
    public static PlayerShoot Instance;
    public PoolManager pm;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
        weaponPower = 1;
    }

    private void Update()
    {
        if (shootingIsActive)
        {
            if (Time.time > nextFire)
            {
                Shoot();
                nextFire = Time.time + 1 / fireRate;
            }
        }
    }

    public void Shoot()
    {
        switch (weaponPower)
        {
            case 1:
                FireProjectile(guns.cGun.transform.position);
                break;
            case 2:
                FireProjectile(guns.lGun.transform.position);
                FireProjectile(guns.rGun.transform.position);
                break;
            case 3:
                FireProjectile(guns.cGun.transform.position);
                FireProjectile(guns.lGun.transform.position);
                FireProjectile(guns.rGun.transform.position);
                break;
        }
    }

    void FireProjectile(Vector3 position)
    {
        GameObject projectile = pm.GetObject(projectilePrefab); // Get a projectile from the pool
        projectile.transform.position = position; // Set position to firing point
        projectile.transform.rotation = Quaternion.identity; // Reset rotation or set as needed
        projectile.SetActive(true); // Activate the projectile
    }

    public void IncreaseWeaponPower()
    {
        weaponPower = Mathf.Min(weaponPower + 1, maxWeaponPower);
    }

}