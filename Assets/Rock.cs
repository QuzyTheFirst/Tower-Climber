using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public static event EventHandler OnRockHitPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            OnRockHitPlayer?.Invoke(this, EventArgs.Empty);
        }
    }
}
