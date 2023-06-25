using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPart : MonoBehaviour
{
    public static event EventHandler OnPlayerHitTopPart;

    private static Transform _player;

    public static void Initialize(Transform player)
    {
        _player = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == _player)
        {
            OnPlayerHitTopPart?.Invoke(this, EventArgs.Empty);
        }
    }
}
