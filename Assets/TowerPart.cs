using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPart : MonoBehaviour
{
    public static event EventHandler OnPlayerHitTopPart;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            OnPlayerHitTopPart?.Invoke(this, EventArgs.Empty);
        }
    }
}
