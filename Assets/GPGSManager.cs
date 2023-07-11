using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSManager : MonoBehaviour
{
    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(AunthenticateCallback);
    }

    internal void AunthenticateCallback(SignInStatus status)
    {
        Debug.Log(status);
    }
}
