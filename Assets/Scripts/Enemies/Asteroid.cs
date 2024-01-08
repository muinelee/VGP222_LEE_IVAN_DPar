using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;

public class Asteroid : MonoBehaviour, IResettable
{
    public int currentHealth;
    public int maxHealth = 1;
    public int damage;
    public int scoreValue = 10;

    public GameObject destructionFX;
    public PoolManager pm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.GetDamage(damage);
        }
    }

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
    }

    private void OnEnable()
    {
        ResetObject();
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        //Debug.Log($"{gameObject.name} took {damage} damage. {currentHealth} health left.");
        if (currentHealth <= 0)
        {
            Destruction();
        }
    }

    void Destruction()
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

    public void ResetObject()
    {
        currentHealth = maxHealth;
    }
}
