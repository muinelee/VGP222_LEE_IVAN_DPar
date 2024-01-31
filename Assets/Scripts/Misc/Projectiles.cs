using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int damage;
    public bool enemyBullet;
    public PoolManager pm; // Make sure to assign this in the inspector!
    private BaseShip bs;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyBullet && collision.CompareTag("Player"))
        {
            bs.GetDamage(damage);
            Destruction(); // Call Destruction directly without checking destroyedByCollision
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