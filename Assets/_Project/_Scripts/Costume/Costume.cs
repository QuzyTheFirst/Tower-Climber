using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Costume : MonoBehaviour
{
    [SerializeField] private ShopManager.CostumeType _costumeType;
    [SerializeField] private int _cost;
    private bool _isBought;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
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

    #region Animations
    public void PlayFallingAnim()
    {
        if(_anim != null)
            _anim.SetTrigger("Fall");
    }

    public void PlayIdleAnim()
    {
        if(_anim != null)
            _anim.SetTrigger("Idle");
    }
    #endregion
}
