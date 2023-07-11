using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSManager : MonoBehaviour
{
    private Achievements _achievements;

    private void Start()
    {
        _achievements = GetComponent<Achievements>();

        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(AunthenticateCallback);
    }

    internal void AunthenticateCallback(SignInStatus status)
    {
        switch (status) 
        {
            case SignInStatus.Success:
                Debug.Log("<color=cyan>Successfully entered Google Play Services</color>");
                break;
            case SignInStatus.Canceled:
                Debug.Log("<color=cyan>Google Play Services canceled connection</color>");
                break;
            case SignInStatus.InternalError:
                Debug.Log("<color=cyan>Internal error while trying to sign in to Google Play Services</color>");
                break;
        }
    }
}
