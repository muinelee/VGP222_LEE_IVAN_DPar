using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager> 
{
    #region Touch Events
    public static event Action<Vector2, float> OnStartTouch;
    public static event Action<Vector2, float> OnEndTouch;
    #endregion

    [HideInInspector] public PlayerInputActions input;

    [SerializeField] private PlayerController pc;

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public Vector2 PrimaryPosition()
    {
        return ScreenToWorld(input.Mobile.PrimaryPosition.ReadValue<Vector2>());
    }

    Vector3 ScreenToWorld(Vector3 pos)
    {
        pos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(pos);
    }

    void Start()
    {
        input.Mobile.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        input.Mobile.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void OnDestroy()
    {
        input.Mobile.PrimaryContact.started -= ctx => StartTouchPrimary(ctx);
        input.Mobile.PrimaryContact.canceled -= ctx => EndTouchPrimary(ctx);
    }

    void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnStartTouch?.Invoke(PrimaryPosition(), (float)ctx.time);
    }

    void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(PrimaryPosition(), (float)ctx.time);
    }
}