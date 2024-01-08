using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject destructionFX;
    public GameObject shield;

    public static PlayerController Instance;

    private Vector3 targetPosition;
    private bool touchActive = false;

    public float moveSpeed = 10.0f;
    public float shieldDuration = 10.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        InputManager.OnStartTouch += StartMovement;
        InputManager.OnEndTouch += StopMovement;
    }

    public void OnDestroy()
    {
        InputManager.OnStartTouch -= StartMovement;
        InputManager.OnEndTouch -= StopMovement;
    }

    public void Update()
    {
        if (touchActive)
        {
            // Update the target position with the current primary touch position
            targetPosition = new Vector3(InputManager.Instance.PrimaryPosition().x, InputManager.Instance.PrimaryPosition().y, transform.position.z);

            // Ensure the game object is always moving towards the updated target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void StartMovement(Vector2 pos, float time)
    {
        touchActive = true;
    }

    void StopMovement(Vector2 pos, float time)
    {
        touchActive = false;
    }

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
