using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public enum MissionType
    {
        MainStory, SideStory, AlternativeStory, Dungeon, Raid, Expedition, EndlessTower
    }

    public enum MissionState
    {
        Locked, Unlocked, InProgress, Completed
    }

    public static MissionManager Instance { get; private set; }
    
    //private EnemyLoader _enemyLoader;
    public static event Action<MissionSO> OnMissionStart;
    public static event Action OnNextWave;
    public static event Action OnMissionComplete;
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

        BattleSystem.OnWaveCompleted += NextWave;
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

    public void StartMission()
    {
        //TODO -> load scene Battle only if energy is used
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
        if (CurrentWave < CurrentMission.Waves.Count)
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
        Debug.Log("Mission completed");
        //TODO -> Update database
        //GiveRewards();
        UnlockNextMission();
        OnMissionComplete?.Invoke();
    }

    private void UnlockNextMission()
    {
        MissionType missionType = CurrentMission.Type;

        if (!_missionLists.TryGetValue(missionType, out MissionSO[] missionList))
        {
            Debug.LogError("Invalid mission type: " + missionType);
            return;
        }

        int completedMissionIndex = Array.IndexOf(missionList, CurrentMission);

        if (completedMissionIndex == -1)
        {
            Debug.LogError("Completed mission not found in the mission list");
            return;
        }

        if (completedMissionIndex + 1 < missionList.Length)
        {
            MissionSO nextMission = missionList[completedMissionIndex + 1];

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
        StartMission();
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
        //List<KeyValuePair<Currency, int>> rewards = CurrentMission.CurrencyRewards.ToList();
        //for (int i = 0; i < CurrentMission.CurrencyRewards.Count; i++)
        //{
        //    string type = rewards[i].Key.ToString();
        //    if (!Enum.TryParse(type, out Currency currencyType)) continue;
        //    PlayFabManager.Instance.AddCurrency(rewards[i].Key, rewards[i].Value);
        //    Debug.Log("Des SOUS !!!");
        //}
        //for (int i = 0; i < CurrentMission.GearReward.Count; i++)
        //{
        //    RewardsDrop.Instance.Drop(CurrentMission);
        //    Debug.Log("Des GEAR !!!");
        //}
        ExperienceSystem.Instance.GainExperienceAccount(CurrentMission.Experience);
        ExperienceSystem.Instance.GainExperiencePlayer(CurrentMission.Experience);
        Debug.Log("De l'XP !!!");
    }
    
    //public List<Enemy> GetMissionEnemyList()
    //{
    //    List<Enemy> MissionEnemies = new List<Enemy>();
    //    for (int i = 0; i < CurrentMission.Enemies.Count; i++)
    //    {
    //        MissionEnemies.Add(_enemyLoader.LoadEnemyByName("Assets/Resources/CSV/Enemies.csv",CurrentMission.Enemies[i]));
    //    }
    //    return MissionEnemies;
    //}
}