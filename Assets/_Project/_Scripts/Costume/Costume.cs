using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class Costume : MonoBehaviour
{
    [Header("Costume")]
    [SerializeField] private ShopManager.CostumeType _costumeType;
    [SerializeField] private int _cost;
    private bool _isBought;

    [Header("IK")]
    [SerializeField] private FastIKFabric _rightArmIK;
    [SerializeField] private FastIKFabric _leftArmIK;

    [Header("Arms Targets")]
    [SerializeField] private Transform _rightArmTarget;
    [SerializeField] private Transform _rightArmPreferedTf;
    [SerializeField] private Transform _leftArmTarget;
    [SerializeField] private Transform _leftArmPreferedTf;

    [Header("Timings")]
    [SerializeField] private float _timeToRestoreArmPos = .25f;
    [SerializeField] private float _distanceBeforeRestoringArmTargetPos = .75f;

    private bool _isRestoringRightArmPos = false;
    private bool _isRestoringLeftArmPos = false;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float towerSpeed = GameManager.Instance.TowerController.TowerSpeed;
        int speedMultiplier = GameManager.Instance.TowerController.IsTowerMoving ? 1 : 0;

        _rightArmTarget.position += Vector3.down * 2 * speedMultiplier * Time.deltaTime;
        _leftArmTarget.position += Vector3.down * 2 * speedMultiplier * Time.deltaTime;

        if(!_isRestoringLeftArmPos && Vector3.Distance(_rightArmTarget.position, _rightArmPreferedTf.position) > _distanceBeforeRestoringArmTargetPos)
        {
            //_rightArmTarget.position = _rightArmPreferedTf.position;
            _isRestoringRightArmPos = true;
            LeanTween.cancel(_rightArmTarget.gameObject);
            LeanTween.move(_rightArmTarget.gameObject, _rightArmPreferedTf.position, _timeToRestoreArmPos).setOnComplete(()=> { _isRestoringRightArmPos = false; });
        }

        if(!_isRestoringRightArmPos && Vector3.Distance(_leftArmTarget.position, _leftArmPreferedTf.position) > _distanceBeforeRestoringArmTargetPos)
        {
            //_leftArmTarget.position = _leftArmPreferedTf.position;
            _isRestoringLeftArmPos = true;
            LeanTween.cancel(_leftArmTarget.gameObject);
            LeanTween.move(_leftArmTarget.gameObject, _leftArmPreferedTf.position, _timeToRestoreArmPos).setOnComplete(()=>{ _isRestoringLeftArmPos = false; });
        }
    }

    public void FallingAnim() => _anim.SetTrigger("Fall");

    public void IdleAnim() => _anim.SetTrigger("Idle");

    public void ClimbingAnim() => _anim.SetTrigger("Climb");

    public void GoInWindowAnim() => _anim.SetTrigger("GoInWindow");

    public void GoOutWindowAnim() => _anim.SetTrigger("GoOutWindow");

    public void UpJumpAnim() 
    {
        _anim.SetTrigger("UpJump"); 
    }
    public void LeftJumpAnim() 
    {
        _anim.SetTrigger("LeftJump"); 
    }
    public void RightJumpAnim() 
    {
        _anim.SetTrigger("RightJump"); 
    }

    public void ToggleIK(bool value)
    {
        _rightArmIK.enabled = value;
        _leftArmIK.enabled = value;
    }

    public void ToggleIKOff()
    {
        _rightArmIK.enabled = false;
        _leftArmIK.enabled = false;
    }

    public void ToggleIKOn()
    {
        _rightArmTarget.position = _rightArmIK.transform.position;
        _leftArmTarget.position = _leftArmIK.transform.position;

        _rightArmIK.enabled = true;
        _leftArmIK.enabled = true;
    }

    public ShopManager.CostumeType CostumeType
    {
        get
        {
            return _costumeType;
        }
    }

    public bool IsBought
    {
        get
        {
            return _isBought;
        }
    }

    public int Cost
    {
        get
        {
            return _cost;
        }
    }

    public void Initialize(bool isBought)
    {
        _isBought = isBought;
    }

    public void Buy()
    {
        _isBought = true;
    }

    public void Restart()
    {
        _isBought = false;
    }
}
