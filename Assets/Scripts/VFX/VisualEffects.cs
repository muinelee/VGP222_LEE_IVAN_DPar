using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour
{
    public float destroyTime;

    private void OnEnable()
    {
        StartCoroutine(Destruction());
    }

    IEnumerator Destruction()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
