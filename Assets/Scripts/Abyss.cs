using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    [SerializeField] private float _abyssSpeed;
    [SerializeField] private float _maxAbyssDistanceFromPlayer = 15;

    private void Update()
    {
        transform.position += Vector3.up * _abyssSpeed * Time.deltaTime;
        
        if(transform.position.y < -_maxAbyssDistanceFromPlayer)
        {
            transform.position = Vector3.down * _maxAbyssDistanceFromPlayer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            GameManager.Instance.KillPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            GameManager.Instance.KillPlayer();
        }
    }
}
