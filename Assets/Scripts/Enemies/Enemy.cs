using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IResettable
{
    public int currentHealth;
    public int maxHealth = 1;
    public int damage;
    public int scoreValue = 25;

    public GameObject destructionFX;
    public PoolManager pm;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.Destruction();
        }
    }

    protected virtual void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
    }

    protected virtual void OnEnable()
    {
        ResetObject();
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destruction();
        }
    }

    protected virtual void Destruction()
    {
        GameManager.Instance.AddScore(scoreValue);
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
        Instantiate(destructionFX, transform.position, Quaternion.identity);
    }

    public virtual void ResetObject()
    {
        currentHealth = maxHealth;
    }
}