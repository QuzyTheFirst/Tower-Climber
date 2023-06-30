using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    [SerializeField] private float _abyssSpeed;

    private void Update()
    {
        transform.position += Vector3.up * _abyssSpeed * Time.deltaTime;
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
