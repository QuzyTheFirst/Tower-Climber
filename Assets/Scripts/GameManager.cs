using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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

    public PlayerController Player { get { return _playerController; } }

    private void OnValidate()
    {
        Application.targetFrameRate = _targetFrameRate;
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

        Application.targetFrameRate = _targetFrameRate;

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
