using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AccountData
{
    public string Name;
    public int Level;
    public int Exp;
    public int Gender;
    public bool Tutorial;
    public List<MissionData> MissionsData;
    public DateTime? LastReward;
    public int LastRewardDay;
    [NonSerialized] public Dictionary<MissionType, List<MissionSO>> MissionsList;

    [NonSerialized] private bool _missingDatas;

    public AccountData(string username)
    {
        Name = username;
        Level = 1;
        Exp = 0;
        Gender = 0;
        Tutorial = false;
        _missingDatas = false;
        LastReward = null;
        LastRewardDay = 0;

        if (PlayFabManager.Instance.IsFirstLogin || PlayFabManager.Instance.IsAccountReset) GetMissionsData();

        MissionManager.OnMissionComplete += CompleteMission;
    }

    ~AccountData()
    {
        MissionManager.OnMissionComplete -= CompleteMission;
    }

    public bool GetMissionsData()
    {
        bool newData = MissionsData == null;
        MissionsData ??= new();
        MissionsList = new();

        foreach (MissionType missionType in Enum.GetValues(typeof(MissionType))) LoadMissionsFromFolder(missionType);
        return !newData && _missingDatas;
    }

    private void LoadMissionsFromFolder(MissionType missionType)
    {
        var missions = Resources.LoadAll<MissionSO>("SO/Missions/" + missionType);

        foreach (var mission in missions)
        {
            MissionData data = MissionsData.Find(x => x.FileName == mission.name);

            if (data == null)
            {
                if (!_missingDatas) _missingDatas = true;

                MissionsData.Add(new()
                {
                    FileName = mission.name,
                    State = mission.State,
                    Unlocked = mission.Unlocked,
                });

                continue;
            }

            mission.UpdateData(data);
        }

        MissionsList.Add(missionType, missions.ToList());
    }

    private void CompleteMission(MissionSO mission)
    {
        Debug.Log($"{mission.Name} completed !");
        mission.State = MissionState.Completed;
        UpdateMissionState(mission);

        int index = MissionsList[mission.Type].IndexOf(mission);

        if (index + 1 < MissionsList[mission.Type].Count)
        {
            mission = MissionsList[mission.Type][index + 1];
            mission.State = MissionState.Unlocked;
            UpdateMissionState(mission);

            Debug.Log($"{mission.Name} unlocked !");
        }

        //if (nextMission.ChapterId != CurrentMission.ChapterId)
        //{
        //    Debug.Log("chapter End");
        //}

        PlayFabManager.Instance.UpdateData();
    }

    private void UpdateMissionState(MissionSO mission)
    {
        Debug.Log(mission.name);
        Debug.Log(MissionsData.Count);
        MissionsData.Find(x => x.FileName == mission.name).State = mission.State;
    }
}