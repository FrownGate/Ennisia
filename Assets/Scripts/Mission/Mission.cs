public class Mission
{
    internal MissionType MissionType;
    internal int EnergyCost;
    internal bool Unlocked;
    internal int DialogueID;

    public MissionSO MissionSO { get; private set; }
    public bool IsCompleted { get; set; }

    // Add any additional properties or fields needed for the mission

    public Mission(MissionSO missionSO)
    {
        MissionSO = missionSO;
        IsCompleted = false;

        // Initialize any additional properties or fields needed for the mission
    }

    // Add any additional methods or logic needed for the mission
}
