using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistance
{
    public enum GameLanguage
    {
        English,
        Russian,
    }
    private GameLanguage _currentGameLanguage = GameLanguage.English;

    public enum MoneyValue
    {
        Up,
        Down,
    }

    public static GameManager Instance;

    [Header("Frames")]
    [SerializeField] private int _targetFrameRate = 60;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TowerController _towerController;

    [Header("Shop Manager")]
    [SerializeField] private ShopManager _shopManager;

    private int _maxScorePoints = 0;
    private int _coins = 0;

    private int _coinsThisMatch = 0;

    public int Coins { get { return _coins; } }

    public ShopManager ShopManager { get { return _shopManager; } }

    public PlayerController Player { get { return _playerController; } }

    private void OnValidate()
    {
        Application.targetFrameRate = _targetFrameRate;
    }

    private void OnEnable()
    {
        Coin.OnPlayerHitCoin += OnCoinCollected;
    }

    private void OnDisable()
    {
        Coin.OnPlayerHitCoin -= OnCoinCollected;
    }

    private void OnCoinCollected(object sender, System.EventArgs e)
    {
        Coin coin = sender as Coin;

        _coins++;
        _coinsThisMatch++;
        GameUIController.Instance.setInGameCoinsText(_coinsThisMatch.ToString());

        Destroy(coin.gameObject);
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ToggleInGamePause(true);
    }

    public void ChangeMoneyValue(int value, MoneyValue moneyValue)
    {
        switch (moneyValue)
        {
            case MoneyValue.Up:
                _coins += value;
                break;
            case MoneyValue.Down:
                _coins -= value;
                break;
        }
    }

    public void ToggleInGamePause(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }

    public void ChangeLanguage(int language)
    {
        _currentGameLanguage = (GameLanguage)language;
        Debug.Log(_currentGameLanguage);
    }

    public void KillPlayer()
    {
        ToggleInGamePause(true);

        if(_towerController.ScorePoints > _maxScorePoints)
        {
            _maxScorePoints = _towerController.ScorePoints;
        }

        GameUIController.Instance.ToggleDeathMenu(true);
        GameUIController.Instance.UpdateDeathMenuUI(_towerController.ScorePoints, _coinsThisMatch);
        GameUIController.Instance.UpdateMainMenuUI(_maxScorePoints, _coins);

        _coinsThisMatch = 0;
    }

    public void ActivateGame()
    {
        ToggleInGamePause(false);
        GameUIController.Instance.ToggleMainMenu(false);
        GameUIController.Instance.ToggleInGameInterface(true);

        GameUIController.Instance.setInGameCoinsText(_coinsThisMatch.ToString());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadData(GameData data)
    {
        _maxScorePoints = data.RecordScorePoints;
        _coins = data.Coins;
        GameUIController.Instance.UpdateMainMenuUI(_maxScorePoints, _coins);
    }

    public void SaveData(GameData data)
    {
        data.RecordScorePoints = _maxScorePoints;
        data.Coins = _coins;
    }
}
