using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI _inGameScoreText;
    [SerializeField] private TextMeshProUGUI _mainMenuScoreText;
    [SerializeField] private TextMeshProUGUI _deathMenuScoreText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _inGameCoinsText;
    [SerializeField] private TextMeshProUGUI _mainMenuCoinsText;
    [SerializeField] private TextMeshProUGUI _deathMenuCoinsText;
     
    [Header("Menus")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _inGameInterface;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _missionsMenu;
    [SerializeField] private GameObject _deathMenu;


    [Header("Sliders")]
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundEffectsVolumeSlider;

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
    }

    // Scores
    public void setInGameScoreText(string text)
    {
        _inGameScoreText.text = text;
    }
    public void setMainMenuScoreText(string text)
    {
        _mainMenuScoreText.text = text;
    }
    public void setDeathMenuScoreText(string text)
    {
        _deathMenuScoreText.text = text;
    }

    // Coins
    public void setInGameCoinsText(string text)
    {
        _inGameCoinsText.text = text;
    }
    public void setMainMenuCoinsText(string text)
    {
        _mainMenuCoinsText.text = text;
    }
    public void setDeathMenuCoinsText(string text)
    {
        _deathMenuCoinsText.text = text;
    }

    // Death Menu
    public void ToggleDeathMenu(bool value) 
    {
        _deathMenu.SetActive(value);
    }

    // Pause Menu
    public void TogglePauseMenu(bool value)
    {
        GameManager.Instance.ToggleInGamePause(value);
        _pauseMenu.SetActive(value);
    }

    public void Continue()
    {
        GameManager.Instance.ToggleInGamePause(false);
        _pauseMenu.SetActive(false);
    }

    public void ToggleSettingsMenu(bool value)
    {
        _settingsMenu.SetActive(value);
    }

    public void GoToMainMenu()
    {
        _mainMenu.SetActive(true);
    }

    // Settings
    public void OnMusicVolumeChange(float value)
    {
        Debug.Log($"Current Music Volume is {_musicVolumeSlider.value}");
    }

    public void OnSoundEffectsVolumeChange(float value)
    {
        Debug.Log($"Current Sound Effects Volume is {_soundEffectsVolumeSlider.value}");
    }

    public void OnLanguageChange(int language)
    {
        GameManager.Instance.ChangeLanguage(language);
    }

    // Main menu
    public void ToggleLeaderboard(bool value)
    {
        Debug.Log($"Leaderboad is {value}");
    }

    public void ToggleMissionsMenu(bool value)
    {
        _missionsMenu.SetActive(value);
    }

    public void ActivateGame()
    {
        _mainMenu.SetActive(false);
        GameManager.Instance.ToggleInGamePause(false);

        _inGameInterface.SetActive(true);
        Debug.Log("Game is activated");
    }
}
