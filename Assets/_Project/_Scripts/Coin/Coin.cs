using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    public static event EventHandler OnPlayerHitCoin;

    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        float randomNumber = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomNumber, 0);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerHitCoin?.Invoke(this, EventArgs.Empty);
        }
    }
}
