using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public static event EventHandler OnPlayerEnterWindow;
    public static event EventHandler OnPlayerExitWindow;

    public bool PlayerHasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerEnterWindow?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerExitWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
