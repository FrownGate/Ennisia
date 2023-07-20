using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowDailyRewards : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _dailyRewardsPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_dailyRewardsPrefab, _panel.transform);

    }
}