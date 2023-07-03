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

    public static GameManager Instance;

    [SerializeField] private int _targetFrameRate = 60;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TowerController _towerController;

    [SerializeField] private int _maxScorePoints = 0;
    [SerializeField] private int _coins = 0;

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
        _coins++;
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
        GameUIController.Instance.ToggleDeathMenu(true);
        GameUIController.Instance.setDeathMenuScoreText($"Score: {_towerController.ScorePoints}");
        if(_towerController.ScorePoints > _maxScorePoints)
        {
            _maxScorePoints = _towerController.ScorePoints;
            GameUIController.Instance.setMainMenuScoreText(_maxScorePoints.ToString());
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadData(GameData data)
    {
        _maxScorePoints = data.RecordScorePoints;
        _coins = data.Coins;
        GameUIController.Instance.setMainMenuScoreText(_maxScorePoints.ToString());
    }

    public void SaveData(GameData data)
    {
        data.RecordScorePoints = _maxScorePoints;
        data.Coins = _coins;
    }
}
