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
    public enum LvlType
    {
        Account, Player
    }
    public LvlType LevelType;
    public LevelUpQuestEvent(int lvl, LvlType type)
    {
        Level = lvl;
        type = LevelType;

    }
}