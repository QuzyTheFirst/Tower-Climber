using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event EventHandler OnPlayerHitCoin;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerHitCoin?.Invoke(this, EventArgs.Empty);
        }
    }
}
