using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    //Buttons
    public void Continue()
    {
        GameManager.Instance.ToggleInGamePause(false);
        GameUIController.Instance.TogglePauseMenu(false);
    }
}
