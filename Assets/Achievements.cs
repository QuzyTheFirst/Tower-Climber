using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Achievements : MonoBehaviour
{
    public void ShowAllAchievments()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void GrantAchievement(string achievement)
    {
        PlayGamesPlatform.Instance.ReportProgress(achievement, 100f, ((isUnlocked) => {
            Debug.Log(isUnlocked);
        }));
    }

    public void DoIncrementalAchievement(string achievement)
    {
        PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;

        platform.IncrementAchievement(achievement, 1, ((isUnlocked) =>
        {
            Debug.Log(isUnlocked);
        }));
    }

    public void RevealAchievement(string achievement)
    {
        Social.ReportProgress(achievement, 0f, ((isUnlocked) => {
            Debug.Log(isUnlocked);
        }));
    }

    public void ListAchievements()
    {
        Social.LoadAchievements(achievements =>
        {
            Debug.Log($"Loaded Achievements: {achievements.Length}");
            foreach(IAchievement ach in achievements)
            {
                Debug.Log($"Achievement: {ach.id}  {ach.completed}");
            }
        });
    }

    public void ListDescription()
    {
        Social.LoadAchievementDescriptions(achievements =>
        {
            foreach (IAchievementDescription ach in achievements)
            {
                Debug.Log($"Achievement: {ach.id}  {ach.title}");
            }
        });
    }


    public void UnlockFirstFallAchievement()
    {
        GrantAchievement(GPGSIds.achievement_first_fall);
    }
}
