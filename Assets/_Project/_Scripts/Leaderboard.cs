using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class Leaderboard
{
    public void PostLeaderboardEntry(int score)
    {
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_leaderboard, (callback) =>
        {
            if(callback == true)
            {
                Debug.Log($"<color=cyan>Score {score} has been posted!</color>");
            }
            else
            {
                Debug.Log("<color=red>Score failed to post</color>");
            }
        });
    }

    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
