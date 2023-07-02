using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int RecordScorePoints;
    public int Coins;

    public Dictionary<string, bool> CostumesCollected;

    public GameData()
    {
        RecordScorePoints = 0;
        Coins = 0;
        CostumesCollected = new Dictionary<string, bool>();
    }
}
