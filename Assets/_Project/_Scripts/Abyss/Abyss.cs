using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    [SerializeField] private float _startingAbyssSpeed = 4;
    [SerializeField] private float _endingAbyssSpeed = 6;

    [SerializeField] private float _startingAbyssDistanceFromPlayer = 16;
    [SerializeField] private float _endingAbyssDistanceFromPlayer = 12;

    [SerializeField] private float _timeToMusterMaxSpeed = 90f;
    [SerializeField] private float _timeToCutDistanceToPlayer = 60f;

    private float _currentAbyssSpeed;
    private float _currentAbyssDistanceFromPlayer;

    private float _maxSpeedChangerTimer = 0f;
    private float _distanceToPlayerChangerTimer = 0f;

    private void Awake()
    {
        _currentAbyssSpeed = _startingAbyssSpeed;
        _currentAbyssDistanceFromPlayer = _startingAbyssDistanceFromPlayer;

        _maxSpeedChangerTimer = 0f;
        _distanceToPlayerChangerTimer = 0f;
    }

    private void Update()
    {
        transform.position += Vector3.up * _currentAbyssSpeed * Time.deltaTime;
        
        if(transform.position.y < -_currentAbyssDistanceFromPlayer)
        {
            transform.position = Vector3.down * _currentAbyssDistanceFromPlayer;
        }

        _maxSpeedChangerTimer += Time.deltaTime;
        _distanceToPlayerChangerTimer += Time.deltaTime;

        _currentAbyssSpeed = Mathf.Lerp(_startingAbyssSpeed, _endingAbyssSpeed, _maxSpeedChangerTimer / _timeToMusterMaxSpeed);
        _currentAbyssDistanceFromPlayer = Mathf.Lerp(_startingAbyssDistanceFromPlayer, _endingAbyssDistanceFromPlayer, _distanceToPlayerChangerTimer / _timeToCutDistanceToPlayer);
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
