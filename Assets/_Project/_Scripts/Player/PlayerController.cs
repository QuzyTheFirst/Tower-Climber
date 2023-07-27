using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _comebackDistance;
    [SerializeField] private float _comebackForce;

    [SerializeField] private float _checkForObstaclesDistance;
    [SerializeField] private LayerMask _obstaclesMask;

    private CostumeSwapper _costumeSwapper;

    private Rigidbody _rig;

    private Vector3 _startingPos;

    private bool _playerGotAway = false;

    private Animator _anim;

    public CostumeSwapper CostumeSwapper
    {
        get
        {
            return _costumeSwapper;
        }
    }

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _startingPos = transform.position;

        _costumeSwapper = GetComponentInChildren<CostumeSwapper>();

        _anim = GetComponent<Animator>();
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

    public void FallingAnim() => _anim.SetTrigger("Fall");

    public void IdleAnim() => _anim.SetTrigger("Idle");

    public void ClimbingAnim() => _anim.SetTrigger("Climb");

    public void GoInWindowAnim() => _anim.SetTrigger("GoInWindow");

    public void GoOutWindowAnim() => _anim.SetTrigger("GoOutWindow");

    public void ToggleIK(bool value) => _costumeSwapper.CurrentCostume.ToggleIK(value);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * _checkForObstaclesDistance);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _checkForObstaclesDistance);
        Gizmos.DrawLine(transform.position, transform.position + -transform.right * _checkForObstaclesDistance);
    }
}
