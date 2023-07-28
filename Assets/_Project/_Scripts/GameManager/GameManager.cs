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

    public enum GameState 
    { 
        InPlay,
        Paused
    }
    private GameState _currentGameState = GameState.Paused;

    public static GameManager Instance;

    [Header("Frames")]
    [SerializeField] private int _targetFrameRate = 60;

    [Header("Camera")]
    [SerializeField] private CameraController _cameraController;

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

    [SerializeField] private int _rewardForPrincess = 5;

    private List<IRestartable> _iRestartableObjs;

    private bool _hasShownStartingCutscene = false;

    public int Coins { get { return _coins; } }
    public int Score { get { return _towerController.ScorePoints; } }

    public TowerController TowerController { get { return _towerController; } }

    public ShopManager ShopManager { get { return _shopManager; } }
    public GPGSManager GPGSManager { get { return _gpgsManager; } }
    public MissionsManager MissionsManager { get { return _missionsManager; } }

    public PlayerController Player { get { return _playerController; } }

    public GameState CurrentGameState { get { return _currentGameState; } }

    public CameraController CameraController { get { return _cameraController; } }

    public int RewardForPrincess { get { return _rewardForPrincess; } }

    private void OnEnable()
    {
        Coin.OnPlayerHitCoin += OnCoinCollected;
        CutsceneController.OnCutsceneEnd += CutsceneUI_OnCutsceneEnd;
    }

    private void OnDisable()
    {
        Coin.OnPlayerHitCoin -= OnCoinCollected;
        CutsceneController.OnCutsceneEnd -= CutsceneUI_OnCutsceneEnd;
    }

    private void CutsceneUI_OnCutsceneEnd(object sender, System.EventArgs e)
    {
        GameUIController.Instance.ToggleCutsceneUI(false);
    }

    private void OnCoinCollected(object sender, System.EventArgs e)
    {
        Coin coin = sender as Coin;

        ChangeMatchMoneyValue(1, MoneyValue.Up);

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

        _cameraController.SwitchCameraToMainMenu();
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

    public void ChangeMatchMoneyValue(int value, MoneyValue moneyValue)
    {
        switch (moneyValue)
        {
            case MoneyValue.Up:
                _coinsThisMatch += value;
                break;
            case MoneyValue.Down:
                _coinsThisMatch -= value;
                break;
        }

        GameUIController.Instance.InGameUI.setCoins(_coinsThisMatch);
    }

    public void ToggleInGamePause(bool value)
    {
        _currentGameState = value ? GameState.Paused : GameState.InPlay;
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
        _cameraController.SwitchCameraToCostumeShop();

        GameUIController.Instance.CostumesShopUI.UpdateUI(_coins);
    }

    public void SwitchCameraToMainMenu()
    {
        _cameraController.SwitchCameraToMainMenu();

        GameUIController.Instance.MainMenuUI.setCoins(_coins);
    }

    public void SwitchCameraToPlayerFalling()
    {
        _cameraController.SwitchCameraToPlayerFalling();
    }

    public void PlayerIdleAnim()
    {
        _playerController.IdleAnim();
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

        _playerController.FallingAnim();

        SwitchCameraToPlayerFalling();
    }

    public void ActivateGame()
    {
        ToggleInGamePause(false);
        GameUIController.Instance.ToggleMainMenu(false);
        GameUIController.Instance.ToggleInGameInterface(true);

        GameUIController.Instance.InGameUI.setCoins(_coinsThisMatch);

        _playerController.ClimbingAnim();
    }

    public void RestartAllRestartables()
    {
        foreach (IRestartable restartableObj in _iRestartableObjs)
        {
            restartableObj.Restart();
        }
    }

    public void ClearSavedData()
    {
        DataPersistanceManager.Instance.NewGame();

        _coins = 0;
        _coinsThisMatch = 0;

        _shopManager.RestartShopManager();
        _missionsManager.RestartMissionsManager();
        GameUIController.Instance.MainMenuUI.UpdateUI(0, 0);

        DataPersistanceManager.Instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        _maxScorePoints = data.RecordScorePoints;
        _coins = data.Coins;
        _hasShownStartingCutscene = data.HasShownStartingCutscene;
        GameUIController.Instance.MainMenuUI.UpdateUI(_maxScorePoints, _coins);

        ChangeLanguage((int)data.SelectedLanguage);

        /*if(_hasShownStartingCutscene == false)
        {
            GameUIController.Instance.ToggleCutsceneUI(true);
            GameUIController.Instance.CutsceneUI.StartShowingCutscene();
        }*/
    }

    public void SaveData(GameData data)
    {
        data.RecordScorePoints = _maxScorePoints;
        data.Coins = _coins;

        data.SelectedLanguage = _currentGameLanguage;

        data.HasShownStartingCutscene = _hasShownStartingCutscene;
    }
}
