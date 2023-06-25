using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public static event EventHandler OnRockHitPlayer;

    public static event EventHandler OnRockHitWindow;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);

        if(other.gameObject.layer == 3)
        {
            OnRockHitPlayer?.Invoke(this, EventArgs.Empty);
        }
        
        if(other.gameObject.layer == 6)
        {
            OnRockHitWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
