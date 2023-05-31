using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static event System.Action<Mission> MissionStarted;
    public static event System.Action<Mission> MissionCompleted;

    // Singleton instance of the MissionManager
    public static MissionManager Instance { get; private set; }

    // List of available missions
    [SerializeField] private HashSet<MissionSO> missions;

    private readonly Dictionary<int, MissionSO> missionDictionary = new Dictionary<int, MissionSO>();
    // Current mission the player is playing
    private Mission currentMission;

    // Energy required to play missions
    private int energy;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        // Populate the mission dictionary
        foreach (MissionSO missionSO in missions)
        {
            missionDictionary.Add(missionSO.ID, missionSO);
        }
    }

    public void StartMission(int missionID)
    {
        if (missionDictionary.TryGetValue(missionID, out MissionSO missionSO))
        {
            Mission mission = new Mission(missionSO);
            currentMission = mission;
            energy -= mission.EnergyCost;

            // Display the mission narrative or cutscene
            DisplayMissionNarrative(mission);

            // Start the mission waves with enemies
            StartMissionWaves(mission);

            // Invoke the MissionStarted event
            MissionStarted?.Invoke(mission);
        }
        else
        {
            Debug.LogError("Mission not found with ID: " + missionID);
        }
    }

    private void StartMissionWaves(Mission mission)
    {
        // Start the mission waves with enemies
        // ...
    }

    public void CompleteMission()
    {
        // Handle mission completion logic

        // Unlock next mission
        UnlockNextMission();

        // Invoke the MissionCompleted event
        MissionCompleted?.Invoke(currentMission);
    }

    private void UnlockNextMission()
    {
        if (missionDictionary.Count > 1)
        {
            if (missionDictionary.TryGetValue(currentMission.MissionSO.ID + 1, out MissionSO nextMissionSO))
            {
                nextMissionSO.Unlocked = true;
            }
            else
            {
                Debug.Log("No more missions available.");
            }
        }
    }

    private void DisplayMissionNarrative(Mission mission)
    {
        // Display the mission narrative or cutscene
        // ...
        if (mission.DialogueID != -1)
        {
            // Show dialogue based on mission's DialogueID
            DialogueManager.Instance.StartDialogue(mission.DialogueID);
        }
    }
}


// Mission Type Enum
public enum MissionType
{
    MainStory,
    SideStory,
    AlternativeStory,
    Dungeon,
    Raid,
    Expedition
}

