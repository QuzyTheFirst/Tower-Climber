using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int RecordScorePoints;
    public int Coins;

    public ShopManager.CostumeType SelectedCostume;
    public Dictionary<ShopManager.CostumeType, bool> CostumesOpened;

    public GameManager.GameLanguage SelectedLanguage;

    public GameData()
    {
        RecordScorePoints = 0;
        Coins = 0;
        SelectedCostume = 0;
        SelectedLanguage = 0;
        CostumesOpened = new Dictionary<ShopManager.CostumeType, bool>();
    }
}
