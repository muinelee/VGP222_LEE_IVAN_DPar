using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public GameObject currentShip;
    private BaseShip baseShip;

    [Header("Ship Database")]
    public ShipDatabase shipDB;
    private int selectedShip = 0;

    [Header("Ship Movement")]
    private Vector3 targetPosition;
    private bool touchActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        LoadSelectedShip();

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
        if (currentShip != null && touchActive)
        {
            float moveSpeed = baseShip.moveSpeed;

            targetPosition = new Vector3(InputManager.Instance.PrimaryPosition().x, InputManager.Instance.PrimaryPosition().y, currentShip.transform.position.z);
            currentShip.transform.position = Vector3.MoveTowards(currentShip.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void LoadSelectedShip()
    {
        int selectedShipIndex = PlayerPrefs.GetInt("SelectedShip", 0);
        ShipSelect selectedShipData = shipDB.GetShip(selectedShipIndex);

        if (selectedShipData.shipPrefab != null)
        {
            if (currentShip != null) Destroy(currentShip);

            currentShip = Instantiate(selectedShipData.shipPrefab);
            baseShip = currentShip.GetComponent<BaseShip>();
            currentShip.tag = "Player";
        }
        else
        {
            Debug.LogError("The ship prefab is not assigned in the ShipSelect object.");
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

    public void SetMovementEnabled(bool enabled)
    {
        touchActive = enabled;
    }
}