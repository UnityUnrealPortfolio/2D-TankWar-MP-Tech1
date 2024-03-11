using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public  event Action<Vector2> OnMoveAction;
    public  event Action<bool> OnFireAction;
    public Vector2 AimInput { get; private set; }

    private Controls controls;

    #region Mono Callbacks
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        } 
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.RemoveCallbacks(this);
    }
    #endregion
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveAction?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
          OnFireAction?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnFireAction?.Invoke(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimInput = context.ReadValue<Vector2>();
    }
}
