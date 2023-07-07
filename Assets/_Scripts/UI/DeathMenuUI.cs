using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenuUI : MonoBehaviour
{
    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void UpdateUI(int scorePoints, int coins)
    {
        setScore(scorePoints);
        setCoins(coins);

    }

    public void setCoins(int coins)
    {
        _coinsText.text = $"Coins: {coins}";
    }

    public void setScore(int scorePoints)
    {
        _scoreText.text = $"Score: {scorePoints}";
    }

    // Buttons
    public void Restart()
    {
        DataPersistanceManager.Instance.SaveGame();

        GameManager.Instance.RestartGame();
    }
}
