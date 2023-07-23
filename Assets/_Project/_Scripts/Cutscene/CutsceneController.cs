using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

[Serializable]
public class DialoguePart
{
    public string Text;
    public bool UseTypeSound = true;

    public Sprite ImageToShow;

    public UnityEvent OnDialoguePartEnd;
}

public class CutsceneController : CutsceneInputHandler
{
    [SerializeField] private TextMeshProUGUI _textOutputField;
    [SerializeField] private Image _imageOutput;

    [SerializeField] private bool _canSkipParts = true;

    [SerializeField] private bool _enableTimer = false;
    [SerializeField] private float _timeBeforeGoingToNextPart = .5f;

    [SerializeField] private UnityEvent OnStart;

    [SerializeField] private int _betweenLetterDelayInMM = 50;

    [SerializeField] private DialoguePart[] _dialogueParts;

    private bool _isCutscenePlaying = false;
    private bool _isShowingDialoguePart = false;

    private bool _skipDialogue;

    private int _currentPart = 0;

    public static event EventHandler OnCutsceneEnd;

    public bool IsCutscenePlaying { get { return _isCutscenePlaying; } }
    public bool CanSkipDialogues { get { return _canSkipParts; } set { _canSkipParts = value; } }

    protected override void Awake()
    {
        base.Awake();

        OnSkip += SkipDialogue;
    }

    private void SkipDialogue(object sender, EventArgs e)
    {
        if (_canSkipParts)
        {
            if (_isShowingDialoguePart)
            {
                _skipDialogue = true;
            }
            else
            {
                _skipDialogue = false;
            }
        }
    }

    private void Start()
    {
        OnStart?.Invoke();
    }

    public void StartShowingCutscene()
    {
        _currentPart = 0;
        if(_currentPart >= _dialogueParts.Length)
        {
            Debug.LogError("Dialogue parts are empty!");
            return;
        }

        ShowDialogueParts();
    }

    private async void ShowDialogueParts()
    {
        _isCutscenePlaying = true;

        while(_currentPart < _dialogueParts.Length)
        {
            await ShowDialoguePart(_dialogueParts[_currentPart]);
            _currentPart++;
        }

        Debug.Log("Cutscene Ended");

        OnCutsceneEnd?.Invoke(this, EventArgs.Empty);

        _isCutscenePlaying = false;
    }

    private async Task ShowDialoguePart(DialoguePart currentDialoguePart)
    {
        _isShowingDialoguePart = true;

        if (_imageOutput != null)
        {
            _imageOutput.sprite = currentDialoguePart.ImageToShow;
        }

        if (_textOutputField != null)
        {
            string fullText = currentDialoguePart.Text;
            string currentText = "";

            for (int i = 0; i <= fullText.Length; i++)
            {
                if (_skipDialogue)
                {
                    _textOutputField.text = fullText;
                    //SoundManager.Instance.Play("Enter");
                    _skipDialogue = false;
                    break;
                }

                currentText = fullText.Substring(0, i);

                /*if(currentDialoguePart.useTypeSound)
                    SoundManager.Instance.Play("Type");*/

                _textOutputField.text = currentText;
                await Task.Delay(_betweenLetterDelayInMM);
            }
        }

        float timer = 0f;

        while (true)
        {
            if (_skipDialogue)
            {
                currentDialoguePart.OnDialoguePartEnd?.Invoke();
                _skipDialogue = false;
                break;
            }

            if (timer > _timeBeforeGoingToNextPart && _enableTimer)
            {
                currentDialoguePart.OnDialoguePartEnd?.Invoke();
                _skipDialogue = false;
                break;
            }

            if(timer > _timeBeforeGoingToNextPart && !_enableTimer && !_canSkipParts)
            {
                currentDialoguePart.OnDialoguePartEnd?.Invoke();
                break;
            }

            timer += .1f;
            await Task.Delay(100);
        }

        _isShowingDialoguePart = false;
    }

    private void ClearCutsceneText()
    {
        if (_textOutputField != null)
            _textOutputField.text = "";
    }

    private void ClearCutsceneImage()
    {
        if (_imageOutput != null)
            _imageOutput.sprite = null;
    }

    private void ClearCutsceneContents()
    {
        if (_textOutputField != null)
            _textOutputField.text = "";

        if (_imageOutput != null)
            _imageOutput.sprite = null;
    }

    #region Public Methods For Modifying Cutscene
    public void ChangeFontAsset(TMP_FontAsset font)
    {
        _textOutputField.font = font;
        _textOutputField.UpdateFontAsset();
    }
    #endregion
}
