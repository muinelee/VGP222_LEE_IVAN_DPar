using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : MonoBehaviour
{
    public float moveSpeed;
    public float shieldDuration;

    public GameObject destructionFX;
    public GameObject shield;

    public void GetDamage(int damage)
    {
        Destruction();
    }

    public void ActivateShield()
    {
        shield.SetActive(true);
        StartCoroutine(DeactivateShield(shieldDuration));
    }

    private IEnumerator DeactivateShield(float duration)
    {
        yield return new WaitForSeconds(duration);
        shield.SetActive(false);
    }

    public void Destruction()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartCoroutine(CallGameOver());
        }
        else
        {
            Debug.Log("GameManager.Instance is null");
        }
        Instantiate(destructionFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator CallGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.GameOver();
    }
}