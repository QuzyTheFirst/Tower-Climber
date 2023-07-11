using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void UpdateUI(int maxScorePoints, int coins)
    {
        setScore(maxScorePoints);
        setCoins(coins);
    }

    public void setScore(int maxScorePoints)
    {
        _scoreText.text = maxScorePoints.ToString();
    }

    public void setCoins(int coins)
    {
        _coinsText.text = coins.ToString();
    }

    //Buttons
    public void ActivateGame()
    {
        GameManager.Instance.ActivateGame();
    }

    public void OpenCostumesShop()
    {
        GameUIController.Instance.ToggleMainMenu(false);
        GameUIController.Instance.ToggleCostumesShop(true);

        GameManager.Instance.SwitchCameraToCostumeShopMenu();
    }

    public void OpenLeaderboard()
    {
        GameUIController.Instance.ToggleMainMenu(false);
        GameUIController.Instance.ToggleLeaderboard(true);

        //GameUIController.Instance.LeaderboardUI.GetLeaderboard();
    }
}