using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int damage;
    public bool enemyBullet;
    public PoolManager pm; // Make sure to assign this in the inspector!
    public BaseShip bs;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
        bs = gameObject.GetComponent<BaseShip>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        if (enemyBullet && playerController != null && collision.gameObject == playerController.currentShip)
        {
            BaseShip ship = playerController.currentShip.GetComponent<BaseShip>();
            if (ship != null)
            {
                ship.GetDamage(damage);
            }
            Destruction();
        }
        else if (collision.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.GetDamage(damage);
                Destruction(); // Call Destruction directly
            }
        }
        else if (!enemyBullet && collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamage(damage);
                Destruction(); // Call Destruction directly
            }
        }
    }

    void Destruction()
    {
        var poolable = GetComponent<PoolableGameObject>();
        if (poolable)
        {
            pm.ReturnObject(gameObject, poolable);
        }
        else
        {
            Debug.LogWarning($"No PoolableGameObject component found on {gameObject.name}. Destroying manually.");
            Destroy(gameObject);
        }
    }
}