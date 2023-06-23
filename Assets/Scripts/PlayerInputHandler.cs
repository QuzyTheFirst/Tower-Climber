using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputs _playerControls;

    protected event EventHandler LeftPartPressPerformed;
    protected event EventHandler LeftPartPressCanceled;
    protected event EventHandler LeftPartMultiTapPerformed;

    protected event EventHandler RightPartPressPerformed;
    protected event EventHandler RightPartPressCanceled;
    protected event EventHandler RightPartMultiTapPerformed;

    protected event EventHandler RestartPerformed;

    protected virtual void Awake()
    {
        _playerControls = new PlayerInputs();

        _playerControls.Map.Left.performed += Left_performed;
        _playerControls.Map.Left.canceled += Left_canceled;

        _playerControls.Map.Right.performed += Right_performed;
        _playerControls.Map.Right.canceled += Right_canceled;

        _playerControls.Map.Restart.performed += Restart_performed;
    }

    private void Restart_performed(InputAction.CallbackContext obj)
    {
        RestartPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Right_canceled(InputAction.CallbackContext context)
    {
        if(context.interaction is PressInteraction)
        {
            RightPartPressCanceled?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Right_performed(InputAction.CallbackContext context)
    {
        if(context.interaction is MultiTapInteraction)
        {
            RightPartMultiTapPerformed?.Invoke(this, EventArgs.Empty);
        }

        if(context.interaction is PressInteraction)
        {
            RightPartPressPerformed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Left_canceled(InputAction.CallbackContext context)
    {
        if(context.interaction is PressInteraction)
        {
            LeftPartPressCanceled?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Left_performed(InputAction.CallbackContext context)
    {
        if(context.interaction is MultiTapInteraction)
        {
            LeftPartMultiTapPerformed?.Invoke(this, EventArgs.Empty);
        }

        if(context.interaction is PressInteraction)
        {
            LeftPartPressPerformed?.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnEnable()
    {
        _playerControls.Enable();
    }

    protected virtual void OnDisable()
    {
        _playerControls.Disable();
    }
}
