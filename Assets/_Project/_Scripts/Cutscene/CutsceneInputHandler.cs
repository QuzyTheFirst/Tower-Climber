using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class CutsceneInputHandler : MonoBehaviour
{
    private CutsceneInputs _cutsceneControls;

    protected event EventHandler OnSkip;

    protected virtual void Awake()
    {
        _cutsceneControls = new CutsceneInputs();
    }


    private void Skip_canceled(InputAction.CallbackContext obj)
    {
        OnSkip?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        _cutsceneControls.Enable();
        _cutsceneControls.Map.Skip.canceled += Skip_canceled;
    }

    private void OnDisable()
    {
        _cutsceneControls.Map.Skip.canceled -= Skip_canceled;
        _cutsceneControls.Disable();
    }
}
