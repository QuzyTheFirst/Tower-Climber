using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundEffectsVolumeSlider;

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
}
