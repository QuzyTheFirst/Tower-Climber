using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _comebackDistance;
    [SerializeField] private float _comebackForce;

    [SerializeField] private float _checkForObstaclesDistance;
    [SerializeField] private LayerMask _obstaclesMask;

    private Rigidbody _rig;

    private Vector3 _startingPos;

    private bool _playerGotAway = false;

    private Renderer _renderer;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _startingPos = transform.position;
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(_startingPos, transform.position);
        if (distance > _comebackDistance)
        {
            _playerGotAway = true;

            Vector3 dir = (_startingPos - transform.position).normalized;

            float additionalForce = distance > 1 ? distance : 1;

            _rig.velocity = dir * _comebackForce * additionalForce * Time.deltaTime;
        }

        if (distance < _comebackDistance && _playerGotAway)
        {
            _playerGotAway = false;

            _rig.velocity = Vector3.zero;

            transform.position = _startingPos;
        }
    }

    public bool CanGoRight()
    {
        return !Physics.Raycast(transform.position, -transform.right, _checkForObstaclesDistance, _obstaclesMask);
    }

    public bool CanGoLeft()
    {
        return !Physics.Raycast(transform.position, transform.right, _checkForObstaclesDistance, _obstaclesMask);
    }

    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * _checkForObstaclesDistance);
        Gizmos.DrawLine(transform.position, transform.position + -transform.right * _checkForObstaclesDistance);
    }
}
