using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MissionType
{
    MainStory, SideStory, AlternativeStory, Dungeon, Raid, Expedition, EndlessTower
}

public enum MissionState
{
    Locked, Unlocked, InProgress, Completed
}

public enum Difficulty
{
    Peaceful, Easy, Normal, Hard, Insane, Ultimate
}

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public static event Action<MissionSO> OnMissionStart;
    public static event Action OnNextWave;
    public static event Action<MissionSO> OnMissionComplete;

    public ChapterSO CurrentChapter { get; private set; }
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
        foreach (MissionType missionType in Enum.GetValues(typeof(MissionType))) LoadMissionsFromFolder(missionType);

        BattleSystem.OnWaveCompleted += NextWave;
    }

    private void LoadMissionsFromFolder(MissionType missionType)
    {
        var missions = Resources.LoadAll<MissionSO>(_path + missionType);

        foreach (var mission in missions)
        {
            //TODO -> Update SO with database datas
        }

        _missionLists.Add(missionType, missions);
    }

    public void StartMission()
    {
        //TODO -> load scene Battle only if energy is used and mission is unlock
        //if (!PlayFabManager.Instance.IsEnergyUsed(CurrentMission.EnergyCost)) return;
        CurrentWave = 1;
        DisplayMissionNarrative(CurrentMission);
        OnMissionStart?.Invoke(CurrentMission);
    }

    public bool IsUnlocked()
    {
        if (CurrentMission.State == MissionState.Unlocked) return true;
        Debug.LogError("Cannot start mission. Mission is either locked or already completed: " + CurrentMission.Id);
        return false;
    }

    public void NextWave()
    {
        Debug.LogWarning($"Next wave - {CurrentMission}");

        if (CurrentMission != null && CurrentWave < CurrentMission.Waves.Count)
        {
            CurrentWave++;
            OnNextWave?.Invoke();
            return;
        }

        CompleteMission();
    }

    public void CompleteMission()
    {
        if (CurrentMission == null)
        {
            Debug.LogError("No mission in progress");
            return;
        }

        CurrentMission.State = MissionState.Completed;
        Debug.LogWarning("Mission completed");
        //TODO -> Update database
        //GiveRewards();
        UnlockNextMission();
        OnMissionComplete?.Invoke(CurrentMission);
        ResetData();
    }

    private void UnlockNextMission()
    {
        var missionType = CurrentMission.Type;

        if (!_missionLists.TryGetValue(missionType, out var missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        var completedMissionIndex = Array.IndexOf(missionList, CurrentMission);

        if (completedMissionIndex == -1)
        {
            Debug.LogError("Completed mission not found in the mission list");
            return;
        }

        if (completedMissionIndex + 1 < missionList.Length)
        {
            var nextMission = missionList[completedMissionIndex + 1];

            if (nextMission.State != MissionState.Locked) return;
            if (nextMission.ChapterId != CurrentMission.ChapterId)
            {
                Debug.Log("chapter End");
            }
            else
            {
                nextMission.State = MissionState.Unlocked;
                Debug.Log("Next mission unlocked: " + nextMission.Id);
                //TODO -> Update database
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
        if (mission.DialogueId != -1) Debug.Log("Displaying mission narrative or cutscene");
        //TODO -> check if narrative is already played (in DialogueManager)
        //DialogueManager.Instance.StartDialogue(mission.DialogueID);
    }

    public void SetChapter(ChapterSO chapter)
    {
        CurrentChapter = chapter;
    }

    public void SetMission(MissionSO mission)
    {
        CurrentMission = mission;
    }

    public List<MissionSO> GetMissionsByChapterId(MissionType missionType, int chapterId)
    {
        if (_missionLists.TryGetValue(missionType, out var missionList))
            return missionList.Where(missionSO => missionSO.ChapterId == chapterId).ToList();
        Debug.LogError("Invalid mission type: " + missionType);
        return new List<MissionSO>();
    }

    public void ResetData()
    {
        CurrentMission = null;
        CurrentChapter = null;
    }

    public bool IsMissionStarted => CurrentMission != null;
    public bool IsFirstWaveCleared => IsMissionStarted && CurrentWave > 1;
}