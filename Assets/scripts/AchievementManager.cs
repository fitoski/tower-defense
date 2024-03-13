using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    protected Callback<UserStatsStored_t> userStatsStored;
    protected Callback<UserAchievementStored_t> userAchievementStored;

    void Start()
    {
        if (SteamManager.Initialized)
        {
            userStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            userAchievementStored = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);
        }

    }

    public void UnlockAchievement(string achievementID)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement(achievementID);
            SteamUserStats.StoreStats();
        }
    }

    private void OnUserStatsStored(UserStatsStored_t callback)
    {
    }

    private void OnUserAchievementStored(UserAchievementStored_t callback)
    {
    }

    void Update()
    {
        
    }
}
