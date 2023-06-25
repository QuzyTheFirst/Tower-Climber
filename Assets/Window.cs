using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public static event EventHandler OnPlayerEnterWindow;
    public static event EventHandler OnPlayerExitWindow;

    private static Transform _playerTf;

    public static void Initialize(Transform playerTf)
    {
        _playerTf = playerTf;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _playerTf)
        {
            OnPlayerEnterWindow?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _playerTf)
        {
            OnPlayerExitWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
