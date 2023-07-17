using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _missionDescriptionField;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderTextField;
    [SerializeField] private TextMeshProUGUI _rewardTextField;
    [SerializeField] private Button _rewardButton;

    public void UpdateUI(Mission mission)
    {
        _missionDescriptionField.text = mission.GetMissionDescription();
        _toggle.isOn = mission.IsCompleted;
        _slider.maxValue = mission.AmountToAchieve;
        _slider.value = mission.GetMissionProgress();
        _sliderTextField.text = $"{mission.GetMissionProgress()} / {mission.AmountToAchieve}";

        _rewardTextField.text = mission.RewardAmount.ToString();

        if (mission.IsCompleted)
        {
            _rewardButton.gameObject.SetActive(true);
        }
        else
        {
            _rewardButton.gameObject.SetActive(false);
        }
    }

    public void CompletedAllMissions()
    {
        _missionDescriptionField.text = "You have completed all missions";
        _toggle.isOn = true;
        _slider.maxValue = 1;
        _slider.value = 1;
        _sliderTextField.text = "You are the best!";
        _rewardTextField.text = "0";
        _rewardButton.gameObject.SetActive(false);
    }

    public void GetRewardBtn()
    {
        GameManager.Instance.MissionsManager.GetRewardForMission();
    }

    public void TurnOff()
    {
        GameUIController.Instance.ToggleMissionsMenu(false);
    }
}
