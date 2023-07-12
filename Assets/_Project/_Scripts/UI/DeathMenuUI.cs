using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class DeathMenuUI : MonoBehaviour
{
    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    [Header("Localization")]
    [SerializeField] private LocalizedString _localStringCoins;
    [SerializeField] private LocalizedString _localStringScore;

    private void OnEnable()
    {
        _localStringCoins.Arguments = new object[] { GameManager.Instance.Coins };
        _localStringCoins.StringChanged += LocalizationCoinsStringChanged;

        _localStringScore.Arguments = new object[] { GameManager.Instance.Score };
        _localStringScore.StringChanged += LocalizationScoreStringChanged;
    }

    private void OnDisable()
    {
        _localStringCoins.StringChanged -= LocalizationCoinsStringChanged;

        _localStringScore.StringChanged -= LocalizationScoreStringChanged;
    }

    private void LocalizationCoinsStringChanged(string value)
    {
        _coinsText.text = value;
    }

    private void LocalizationScoreStringChanged(string value)
    {
        _scoreText.text = value;
    }

    public void UpdateUI(int scorePoints, int coins)
    {
        setScore(scorePoints);
        setCoins(coins);
    }

    public void setCoins(int coins)
    {
        _localStringCoins.Arguments[0] = coins;
        _localStringCoins.RefreshString();
    }

    public void setScore(int scorePoints)
    {
        _localStringScore.Arguments[0] = scorePoints;
        _localStringScore.RefreshString();
    }

    // Buttons
    public void ToMainMenuBtn()
    {
        GameUIController.Instance.ToggleDeathMenu(false);
        GameUIController.Instance.ToggleMainMenu(true);
    }

    public void RestartGameBtn()
    {
        GameUIController.Instance.ToggleDeathMenu(false);

        GameManager.Instance.ActivateGame();
    }
}
