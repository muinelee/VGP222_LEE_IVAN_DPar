using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldupScript : MonoBehaviour
{
    public PoolManager pm;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().ActivateShield();

            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        var poolable = GetComponent<PoolableGameObject>();

        pm.ReturnObject(gameObject, poolable);
    }
}