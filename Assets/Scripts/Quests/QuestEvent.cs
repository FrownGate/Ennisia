using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestEvent
{
    public string EventDescription;
}

public class KillQuestEvent : QuestEvent
{
    public string KilledName;

    public KillQuestEvent(string name)
    {
        KilledName = name;
    }
}

public class MissionQuestEvent : QuestEvent
{
    public MissionSO Mission;

    public MissionQuestEvent(MissionSO mission)
    {
        Mission = mission;
    }
}
public class LevelUpQuestEvent : QuestEvent
{
    public int Level;

    public LevelUpQuestEvent(int lvl)
    {
        Level = lvl;
    }
}