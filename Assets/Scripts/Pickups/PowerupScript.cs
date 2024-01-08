using Unity.VisualScripting;
using UnityEngine;

public class PowerupScript : MonoBehaviour
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
            collision.gameObject.GetComponent<PlayerShoot>().IncreaseWeaponPower();
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        var poolable = GetComponent<PoolableGameObject>();
        pm.ReturnObject(gameObject, poolable);
    }
}
