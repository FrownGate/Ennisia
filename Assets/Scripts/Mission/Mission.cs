public class Mission
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public MissionType MissionType { get; private set; }
    public int EnergyCost { get; private set; }
    public MissionState State { get; set; }
    public int DialogueID { get; private set; }

    // Add any additional properties or fields needed for the mission

    public Mission(MissionSO missionSO)
    {
        ID = missionSO.ID;
        Name = missionSO.Name;
        MissionType = missionSO.MissionType;
        EnergyCost = missionSO.EnergyCost;
        State = missionSO.Unlocked ? MissionState.Unlocked : MissionState.Locked;
        DialogueID = missionSO.DialogueID;
        // Initialize any additional properties or fields needed for the mission
    }

    // Add any additional methods or logic needed for the mission
}

public enum MissionType
{
    MainStory, SideStory, AlternativeStory, Dungeon, Raid, Expedition
}

public enum MissionState
{
    Locked, Unlocked, InProgress, Completed
}