using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static event Action<Mission> MissionStarted;
    public static event Action<Mission> MissionCompleted;

    public static MissionManager Instance { get; private set; }

    private readonly Dictionary<MissionType, List<Mission>> _missionLists = new();
    public Mission CurrentMission;
    public int CurrentWave;
    private int _energy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        string path = "SO/Missions/";

        foreach (MissionType missionType in Enum.GetValues(typeof(MissionType)))
        {
            LoadMissionsFromFolder(path, missionType);
        }
    }

    private void LoadMissionsFromFolder(string folderName, MissionType missionType)
    {
        MissionSO[] missionSOs = Resources.LoadAll<MissionSO>(folderName + missionType);
        List<Mission> missionList = new List<Mission>();

        foreach (MissionSO missionSO in missionSOs)
        {
            Mission mission = new Mission(missionSO);
            missionList.Add(mission);
        }

        _missionLists.Add(missionType, missionList);
    }

    public void StartMission(MissionType missionType, int missionID)
    {
        if (!_missionLists.TryGetValue(missionType, out List<Mission> missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        Mission mission = missionList.Find(m => m.ID == missionID);

        if (mission == null)
        {
            Debug.LogError("Mission not found with ID: " + missionID);
            return;
        }

        if (mission.State != MissionState.Unlocked)
        {
            Debug.LogError("Cannot start mission. Mission is either locked or already completed: " + missionID);
            return;
        }

        CurrentMission = mission;
        _energy -= mission.EnergyCost;

        DisplayMissionNarrative(mission);

        StartMissionWaves(mission);

        MissionStarted?.Invoke(mission);
    }

    private void StartMissionWaves(Mission mission)
    {
        // Start the mission waves with enemies
        // ...
    }

    public void CompleteMission()
    {
        if (CurrentMission == null)
        {
            Debug.LogError("No mission in progress");
            return;
        }

        CurrentMission.State = MissionState.Completed;
        Debug.Log("Mission completed");

        UnlockNextMission(CurrentMission);

        MissionCompleted?.Invoke(CurrentMission);
    }

    private void UnlockNextMission(Mission completedMission)
    {
        MissionType missionType = completedMission.MissionType;

        if (!_missionLists.TryGetValue(missionType, out List<Mission> missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        int completedMissionIndex = missionList.IndexOf(completedMission);

        if (completedMissionIndex == -1)
        {
            Debug.LogError("Completed mission not found in the mission list");
            return;
        }

        if (completedMissionIndex + 1 < missionList.Count)
        {
            Mission nextMission = missionList[completedMissionIndex + 1];

            if (nextMission.State == MissionState.Locked)
            {
                nextMission.State = MissionState.Unlocked;
                Debug.Log("Next mission unlocked: " + nextMission.ID);
            }
        }
        else
        {
            Debug.Log("No more missions available in the " + missionType + " category.");
        }
    }

    private void DisplayMissionNarrative(Mission mission)
    {
        // Display the mission narrative or cutscene
        // ...
        if (mission.DialogueID != -1)
        {
            // Show dialogue based on mission's DialogueID
            //DialogueManager.Instance.StartDialogue(mission.DialogueID);
            Debug.Log("Displaying mission narrative or cutscene");
        }
    }
}

public enum MissionType
{
    MainStory,
    SideStory,
    AlternativeStory,
    Dungeon,
    Raid,
    Expedition
}

public enum MissionState
{
    Locked,
    Unlocked,
    InProgress,
    Completed
}
