using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mission
{
    public MissionsManager.MissionType _missionType;

    public int AmountToAchieve;
    public int RewardAmount;

    private bool _isCompleted = false;

    private int _achievedAmount = 0;

    public bool IsCompleted { get { return _isCompleted; } }

    public int AchievedAmount { get { return _achievedAmount; }  set { _achievedAmount = value; } }

    public void UpdateMissionData(RunData data)
    {
        AddAchievedAmount(data);

        Debug.Log($"Achieved Amount: {_achievedAmount} | Amount to achieve: {AmountToAchieve} | isCompleted: {_isCompleted}");
    }

    private void AddAchievedAmount(RunData data)
    {
        switch (_missionType)
        {
            case MissionsManager.MissionType.AchieveScore:
                if(data.Score > _achievedAmount)
                    _achievedAmount = data.Score;
                break;

            case MissionsManager.MissionType.CollectCoins:
                _achievedAmount += data.Coins;
                break;

            case MissionsManager.MissionType.CollectCoinsInOneRun:
                if(data.Coins > _achievedAmount)
                    _achievedAmount = data.Coins;
                break;

            case MissionsManager.MissionType.HideInWindows:
                _achievedAmount += data.TimesHidenInWindows;
                break;

            case MissionsManager.MissionType.HideInWindowsInOneRun:
                if(data.TimesHidenInWindows > _achievedAmount)
                    _achievedAmount = data.TimesHidenInWindows;
                break;
            default:
                Debug.LogError($"Mission Type {_missionType} is not defined");
                break;
        }

        if (_achievedAmount > AmountToAchieve)
            _achievedAmount = AmountToAchieve;

        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (_achievedAmount >= AmountToAchieve)
            _isCompleted = true;
        else
            _isCompleted = false;
    }

    public string GetMissionDescription()
    {
        switch (_missionType)
        {
            case MissionsManager.MissionType.AchieveScore:
                return $"Achieve {AmountToAchieve} score";

            case MissionsManager.MissionType.CollectCoins:
                return $"Collect {AmountToAchieve} coins";

            case MissionsManager.MissionType.CollectCoinsInOneRun:
                return $"Collect {AmountToAchieve} coins in one run";

            case MissionsManager.MissionType.HideInWindows:
                return $"Hide in windows {AmountToAchieve} times";

            case MissionsManager.MissionType.HideInWindowsInOneRun:
                return $"Hide in windows {AmountToAchieve} times in one run";
        }

        return "No description was found";
    }

    public int GetMissionProgress()
    {
        return _achievedAmount;
    }
}
