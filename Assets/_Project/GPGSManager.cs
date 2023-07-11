using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSManager : MonoBehaviour
{
    private Achievements _achievements;
    private Leaderboard _leaderboard;

    public Achievements Achievement { get { return _achievements; } }
    public Leaderboard Leaderboard { get { return _leaderboard; } }

    private void Start()
    {
        _achievements = GetComponent<Achievements>();
        _leaderboard = GetComponent<Leaderboard>();

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
                Debug.Log("<color=red>Google Play Services canceled connection</color>");
                break;
            case SignInStatus.InternalError:
                Debug.Log("<color=red>Internal error while trying to sign in to Google Play Services</color>");
                break;
        }
    }
}
