using Unity.VisualScripting;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public PoolManager pm;

    private void Awake()
    {
        pm = FindObjectOfType<PoolManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the object is already inactive, don't do anything. Fixes a bug where the object would be returned to the pool too early.
        if (!collision.gameObject.activeInHierarchy) return;

        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<BaseShip>().Destruction();
            return;
        }

        if (collision.CompareTag("Asteroid") || collision.CompareTag("Projectile") || collision.CompareTag("Enemy") || collision.CompareTag("Powerup") || collision.CompareTag("Blackhole"))
        {
            var poolable = collision.GetComponent<PoolableGameObject>();
            if (poolable)
            {
                pm.ReturnObject(collision.gameObject, poolable);
            }
            else
            {
                Debug.LogWarning($"Object {collision.name} exited the boundary but has no PoolableGameObject component. Destroying object.");
                Destroy(collision.gameObject);
            }
        }
    }
}