using System;
using UnityEngine;

public class DailyRewardCheck : MonoBehaviour
{
    private DateTime? _lastReward;

    private void Awake()
    {
        if (PlayFabManager.Instance.DailiesCheck) return;
        PlayFabManager.Instance.DailiesCheck = true;

        _lastReward = PlayFabManager.Instance.Account.LastReward;

        if (_lastReward == null || _lastReward.Value.Date < DateTime.Now)
        {
            Debug.Log("Daily Reward");
            //TODO -> update LastReward
        }
    }
}