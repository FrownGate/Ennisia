using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public enum MissionType
    {
        MainStory,
        SideStory,
        AlternativeStory,
        Dungeon,
        Raid,
        Expedition,
        EndlessTower
    }

    public enum MissionState
    {
        Locked,
        Unlocked,
        InProgress,
        Completed
    }

    public static MissionManager Instance { get; private set; }
    public static event Action<MissionSO> OnMissionStart; //Not used yet
    public static event Action<MissionSO> OnMissionComplete; //Not used yet
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

        if (!PlayFabManager.Instance.IsEnergyUsed(mission.EnergyCost)) return;
        CurrentWave = 1;

        DisplayMissionNarrative(CurrentMission);
        StartMission(CurrentMission);
        OnMissionStart?.Invoke(CurrentMission);
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

    public void CompleteMission()
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
        GiveRewards();
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

            if (nextMission.State != MissionState.Locked) return;
            if (nextMission.ChapterId != completedMission.ChapterId)
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

    public void SetMission(MissionSO mission)
    {
        CurrentMission = mission;
    }

    public List<MissionSO> GetMissionsByChapterId(MissionType missionType, int chapterId)
    {
        if (_missionLists.TryGetValue(missionType, out MissionSO[] missionList))
            return missionList.Where(missionSO => missionSO.ChapterId == chapterId).ToList();
        Debug.LogError("Invalid mission type: " + missionType);
        return new List<MissionSO>();

    }

    public void ResetMissionManager()
    {
        CurrentMission = null;
        CurrentChapter = null;
    }

    public void GiveRewards()
    {
        List<KeyValuePair<PlayFabManager.GameCurrency, int>> rewards = CurrentMission.RewardsList.ToList();
        for (int i = 0; i < CurrentMission.RewardsList.Count; i++)
        {
            string type = rewards[i].Key.ToString();
            if (!Enum.TryParse(type, out PlayFabManager.GameCurrency currencyType)) continue;
            PlayFabManager.Instance.AddCurrency(rewards[i].Key, rewards[i].Value);
            Debug.Log("Des SOUS !!!");
        }

        ExperienceSystem.Instance.GainExperienceAccount(CurrentMission.Experience);
        ExperienceSystem.Instance.GainExperiencePlayer(CurrentMission.Experience);
        Debug.Log("De l'XP !!!");
    }
}