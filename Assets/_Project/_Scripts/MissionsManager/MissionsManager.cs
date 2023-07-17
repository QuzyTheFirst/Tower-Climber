using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour, IDataPersistance
{
    public enum MissionType
    {
        CollectCoins,
        CollectCoinsInOneRun,
        AchieveScore,
        HideInWindows,
        HideInWindowsInOneRun,
    }

    [SerializeField] private Mission[] _missions;

    private int _currentMission;

    public Mission CurrentMission { get { return _missions[_currentMission]; } }

    public bool CompletedAllMissions { get { return _currentMission >= _missions.Length; } }

    private void Start()
    {
        if (CompletedAllMissions)
            return;

        GameUIController.Instance.MissionsUI.UpdateUI(CurrentMission);
    }

    public void ProcessMissionData(RunData data)
    {
        if (CompletedAllMissions)
            return;

        CurrentMission.UpdateMissionData(data);

        GameUIController.Instance.MissionsUI.UpdateUI(CurrentMission);
    }

    public void GetRewardForMission()
    {
        if (!CurrentMission.IsCompleted)
            return;

        GameManager.Instance.ChangeMoneyValue(CurrentMission.RewardAmount, GameManager.MoneyValue.Up);

        GetNextMission();

        DataPersistanceManager.Instance.SaveGame();
    }

    private void GetNextMission()
    {
        if (!CurrentMission.IsCompleted)
            return;

        _currentMission++;

        if (CompletedAllMissions)
        {
            GameUIController.Instance.MissionsUI.CompletedAllMissions();
            return;
        }
        else
        {
            GameUIController.Instance.MissionsUI.UpdateUI(CurrentMission);
        }
    }

    public void LoadData(GameData data)
    {
        _currentMission = data.CurrentMission;

        if (CompletedAllMissions)
        {
            GameUIController.Instance.MissionsUI.CompletedAllMissions();
            return;
        }

        CurrentMission.AchievedAmount = data.MissionAchievedAmount;
        GameUIController.Instance.MissionsUI.UpdateUI(CurrentMission);
    }

    public void SaveData(GameData data)
    {
        data.CurrentMission = _currentMission;

        if (CompletedAllMissions)
        {
            data.MissionAchievedAmount = 0;
            return;
        }

        data.MissionAchievedAmount = CurrentMission.AchievedAmount;
    }
}
