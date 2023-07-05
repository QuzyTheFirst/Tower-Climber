using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    [Header("Menus")]
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private MainMenuUI _mainMenuUI;
    [SerializeField] private MissionsUI _missionsUI;
    [SerializeField] private DeathMenuUI _deathMenuUI;
    [SerializeField] private CostumesShopUI _costumesShopUI;

    public PauseUI PauseUI { get { return _pauseUI; } }
    public InGameUI InGameUI { get { return _inGameUI; } }
    public SettingsUI SettingsUI { get { return _settingsUI; } }
    public MainMenuUI MainMenuUI { get { return _mainMenuUI; } }
    public MissionsUI MissionsUI { get { return _missionsUI; } }
    public DeathMenuUI DeathMenuUI { get { return _deathMenuUI; } }
    public CostumesShopUI CostumesShopUI { get { return _costumesShopUI; } }


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

    public void ToggleDeathMenu(bool value) 
    {
        _deathMenuUI.gameObject.SetActive(value);
    }

    public void TogglePauseMenu(bool value)
    {
        _pauseUI.gameObject.SetActive(value);
    }

    public void ToggleSettingsMenu(bool value)
    {
        _settingsUI.gameObject.SetActive(value);
    }

    public void ToggleLeaderboard(bool value)
    {
        Debug.Log($"Leaderboad is {value}");
    }

    public void ToggleMissionsMenu(bool value)
    {
        _missionsUI.gameObject.SetActive(value);
    }

    public void ToggleMainMenu(bool value)
    {
        _mainMenuUI.gameObject.SetActive(value);
    }

    public void ToggleInGameInterface(bool value)
    {
        _inGameUI.gameObject.SetActive(value);
    }
    public void ToggleCostumesShop(bool value)
    {
        _costumesShopUI.gameObject.SetActive(value);
    }
}
