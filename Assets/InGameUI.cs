using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void UpdateUI(int maxScorePoints, int coins)
    {
        setScore(maxScorePoints);
        setCoins(coins);
    }

    public void setScore(int scorePoints)
    {
        _scoreText.text = scorePoints.ToString();
    }

    // Coins
    public void setCoins(int coins)
    {
        _coinsText.text = coins.ToString();
    }

    public void PauseGame()
    {
        GameManager.Instance.ToggleInGamePause(true);
        GameUIController.Instance.TogglePauseMenu(true);
    }
}
