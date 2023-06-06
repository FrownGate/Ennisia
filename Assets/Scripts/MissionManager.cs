using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public enum MissionType
    {
        MainStory, SideStory, AlternativeStory, Dungeon, Raid, Expedition
    }

    public enum MissionState
    {
        Locked, Unlocked, InProgress, Completed
    }

    public static MissionManager Instance { get; private set; }
    public static event Action<MissionSO> OnMissionStart; //Not used yet
    public static event Action<MissionSO> OnMissionComplete; //Not used yet

    public ChapterSO CurrentChapter;
    public MissionSO CurrentMission { get; private set; }
    public int CurrentWave { get; private set; }

    private readonly Dictionary<MissionType, MissionSO[]> _missionLists = new();
    private readonly string _path = "SO/Missions/";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Init missions list with MissionSO
        foreach (MissionType missionType in Enum.GetValues(typeof(MissionType)))
        {
            LoadMissionsFromFolder(missionType);
        }
    }

    private void LoadMissionsFromFolder(MissionType missionType)
    {
        MissionSO[] missions = Resources.LoadAll<MissionSO>(_path + missionType);

        foreach (MissionSO mission in missions)
        {
            //TODO -> Update SO with database datas
        }

        _missionLists.Add(missionType, missions);
    }

    public void StartMission(MissionType missionType, int missionID)
    {
        if (!_missionLists.TryGetValue(missionType, out MissionSO[] missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        MissionSO mission = Array.Find(missionList, m => m.Id == missionID);

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

        if (PlayFabManager.Instance.EnergyIsUsed(mission.EnergyCost))
        {
            CurrentMission = mission;
            CurrentWave = 1;

            DisplayMissionNarrative(CurrentMission);
            StartMission(CurrentMission);
            OnMissionStart?.Invoke(CurrentMission);
        }
    }

    private void StartMission(MissionSO mission)
    {
        // Start the mission waves with enemies
        //TODO -> Load Battle scene

    }

    public bool NextWave()
    {
        if (CurrentWave < CurrentMission.Waves.Count)
        {
            CurrentWave++;
            return true;
        }
        else
        {
            CompleteMission();
            return false;
        }
    }

    private void CompleteMission()
    {
        //TODO -> Load Main Menu Scene, maybe end mission popup
        if (CurrentMission == null)
        {
            Debug.LogError("No mission in progress");
            return;
        }

        CurrentMission.State = MissionState.Completed;
        Debug.Log("Mission completed");
        //TODO -> Update database

        UnlockNextMission(CurrentMission);
        OnMissionComplete?.Invoke(CurrentMission);
    }

    private void UnlockNextMission(MissionSO completedMission)
    {
        MissionType missionType = completedMission.Type;

        if (!_missionLists.TryGetValue(missionType, out MissionSO[] missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        int completedMissionIndex = Array.IndexOf(missionList, completedMission);

        if (completedMissionIndex == -1)
        {
            Debug.LogError("Completed mission not found in the mission list");
            return;
        }

        if (completedMissionIndex + 1 < missionList.Length)
        {
            MissionSO nextMission = missionList[completedMissionIndex + 1];

            if (nextMission.State == MissionState.Locked)
            {
                if (nextMission.ChapterId != completedMission.ChapterId) { Debug.Log("chapter End"); }
                else
                {
                    nextMission.State = MissionState.Unlocked;
                    Debug.Log("Next mission unlocked: " + nextMission.Id);
                    //TODO -> Update database

                }
            }
        }
        else
        {
            Debug.Log("No more missions available in the " + missionType + " category.");
        }
    }

    private void DisplayMissionNarrative(MissionSO mission)
    {
        // Display the mission narrative or cutscene
        if (mission.DialogueId != -1)
        {
            Debug.Log("Displaying mission narrative or cutscene");
            //TODO -> check if narrative is already played (in DialogueManager)
            //DialogueManager.Instance.StartDialogue(mission.DialogueID);
        }
    }
    public void SetChapter(ChapterSO chapter)
    {
        CurrentChapter = chapter;
    }
    public List<MissionSO> GetMissionsByChapterId(MissionType missionType, int chapterId)
    {
        if (!_missionLists.TryGetValue(missionType, out MissionSO[] missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return new List<MissionSO>();
        }

        List<MissionSO> missions = new();

        foreach (MissionSO missionSO in missionList)
        {
            if (missionSO.ChapterId == chapterId)
            {
                missions.Add(missionSO);
            }
        }

        return missions;
    }


}