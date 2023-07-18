using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Costume : MonoBehaviour
{
    [SerializeField] private ShopManager.CostumeType _costumeType;
    [SerializeField] private int _cost;
    private bool _isBought;

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
