using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, IDataPersistance
{
    public enum CostumeType
    {
        Green,
        Blue,
        Red,
    }

    [Header("Shop")]
    [SerializeField] private Shop _shop;

    [Header("Costumes")]
    [SerializeField] private Costume[] _allCostumes;

    private Costume _currentChosenCostume;

    [Header("Game Manager")]
    [SerializeField] private GameManager _gameManager;

    public int CostumesCount { get { return _allCostumes.Length; } }
    public Costume SelectedCostume { get { return _currentChosenCostume; } }
    public Shop Shop { get { return _shop; } }

    private void Start()
    {
        if (!GetCostume(0).IsBought)
            GetCostume(0).Buy();

        ChangeSelectedCostume(0);
    }

    public void ChangeSelectedCostume(CostumeType costumeType)
    {
        Costume costume = Array.Find(_allCostumes, costume => costume.CostumeType == costumeType);

        if(costume == null)
        {
            Debug.LogError($"Error! Costume of type {costumeType} wasn't found!");
        }

        _gameManager.Player.CostumeSwapper.ChangeCostume(costume.transform);

        _currentChosenCostume = costume;
    }

    public bool BuySelectCostume(CostumeType costumeType)
    {
        Costume costume = Array.Find(_allCostumes, costume => costume.CostumeType == costumeType);
        if(costume != null)
        {
            if (!costume.IsBought)
            {
                if (_gameManager.Coins > costume.Cost)
                {
                    costume.Buy();
                    _gameManager.ChangeMoneyValue(costume.Cost, GameManager.MoneyValue.Down);
                    ChangeSelectedCostume(costume.CostumeType);
                    return true;
                }
            }
            else
            {
                ChangeSelectedCostume(costume.CostumeType);
                return true;
            }
        }
        return false;
    }

    public Costume GetCostume(CostumeType costumeType)
    {
        Costume costume = Array.Find(_allCostumes, costume => costume.CostumeType == costumeType);
        return costume;
    }

    public void LoadData(GameData data)
    {
        foreach (Costume costume in _allCostumes)
        {
            bool isBought;
            data.CostumesOpened.TryGetValue(costume.CostumeType, out isBought);
            costume.Initialize(isBought);
        }

        ChangeSelectedCostume(data.SelectedCostume);
    }

    public void SaveData(GameData data)
    {
        foreach (Costume costume in _allCostumes)
        {
            if (data.CostumesOpened.ContainsKey(costume.CostumeType))
            {
                data.CostumesOpened.Remove(costume.CostumeType);
            }

            data.CostumesOpened.Add(costume.CostumeType, costume.IsBought);
        }

        data.SelectedCostume = _currentChosenCostume.CostumeType;
    }
}
