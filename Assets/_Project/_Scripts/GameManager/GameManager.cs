using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour, IDataPersistance
{
    public enum GameLanguage
    {
        English = 0,
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

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TowerController _towerController;

    [Header("Shop Manager")]
    [SerializeField] private ShopManager _shopManager;

    [Header("GPGS Manager")]
    [SerializeField] private GPGSManager _gpgsManager;

    [Header("Missions Manager")]
    [SerializeField] private MissionsManager _missionsManager;

    private int _maxScorePoints = 0;
    private int _coins = 0;

    private int _coinsThisMatch = 0;

    private List<IRestartable> _iRestartableObjs;

    public int Coins { get { return _coins; } }
    public int Score { get { return _towerController.ScorePoints; } }

    public ShopManager ShopManager { get { return _shopManager; } }
    public GPGSManager GPGSManager { get { return _gpgsManager; } }
    public MissionsManager MissionsManager { get { return _missionsManager; } }

    public PlayerController Player { get { return _playerController; } }

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

        _coinsThisMatch++;
        GameUIController.Instance.InGameUI.setCoins(_coinsThisMatch);

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

        Application.targetFrameRate = _targetFrameRate;
        _iRestartableObjs = Helper.FindInterfacesOfType<IRestartable>(true).ToList();
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

        GameUIController.Instance.MainMenuUI.setCoins(_coins);
        GameUIController.Instance.CostumesShopUI.UpdateUI(_coins);
        DataPersistanceManager.Instance.SaveGame();
    }

    public void ToggleInGamePause(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }

    public void ChangeLanguage(int language)
    {
        _currentGameLanguage = (GameLanguage)language;
        StartCoroutine(SetLocale(language));
    }

    IEnumerator SetLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
    }

    public void SwitchCameraToCostumeShopMenu()
    {
        _camera.transform.position = _shopManager.Shop.CameraPosition;
        _camera.LookAt = _shopManager.Shop.CameraLookAtObject;

        GameUIController.Instance.CostumesShopUI.UpdateUI(_coins);
    }

    public void SwitchCameraToMainMenu()
    {
        _camera.transform.position = _towerController.CameraPosition;
        _camera.LookAt = _towerController.CameraLookAtTf;

        GameUIController.Instance.MainMenuUI.setCoins(_coins);
    }

    public void KillPlayer()
    {
        ToggleInGamePause(true);

        RunData runData = new RunData()
        {
            Coins = _coinsThisMatch,
            Score = _towerController.ScorePoints,
            TimesHidenInWindows = _towerController.TimesHidenInWindows
        };
        Debug.Log($"Coins: {runData.Coins} | Score: {runData.Score} | TimesHidenInWindow: {runData.TimesHidenInWindows}");
        _missionsManager.ProcessMissionData(runData);

        if(runData.Score > _maxScorePoints)
        {
            _maxScorePoints = runData.Score;
            GameUIController.Instance.MainMenuUI.setScore(_maxScorePoints);
            _gpgsManager.Leaderboard.PostLeaderboardEntry(_maxScorePoints);
        }

        GameUIController.Instance.ToggleInGameInterface(false);
        GameUIController.Instance.ToggleDeathMenu(true);
        GameUIController.Instance.DeathMenuUI.UpdateUI(runData.Score, runData.Coins);

        ChangeMoneyValue(runData.Coins, MoneyValue.Up);

        _coinsThisMatch = 0;

        RestartAllRestartables();
    }

    public void ActivateGame()
    {
        ToggleInGamePause(false);
        GameUIController.Instance.ToggleMainMenu(false);
        GameUIController.Instance.ToggleInGameInterface(true);

        GameUIController.Instance.InGameUI.setCoins(_coinsThisMatch);
    }

    public void RestartAllRestartables()
    {
        foreach (IRestartable restartableObj in _iRestartableObjs)
        {
            restartableObj.Restart();
        }
    }

    public void LoadData(GameData data)
    {
        _maxScorePoints = data.RecordScorePoints;
        _coins = data.Coins;
        GameUIController.Instance.MainMenuUI.UpdateUI(_maxScorePoints, _coins);

        ChangeLanguage((int)data.SelectedLanguage);
    }

    public void SaveData(GameData data)
    {
        data.RecordScorePoints = _maxScorePoints;
        data.Coins = _coins;

        data.SelectedLanguage = _currentGameLanguage;
    }
}
