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

public class GearLevelMaxQuestEvent : QuestEvent
{
    public GearType? Type;
    public GearLevelMaxQuestEvent(GearType? type)
    {
        Type = type;
    }
}
public class DefeatQuestEvent : QuestEvent
{
    public bool Lost;
    public DefeatQuestEvent(bool lost)
    {
        Lost = lost;
    }
}
public class EnergyQuestEvent : QuestEvent
{
    public int Amount;
    public EnergyQuestEvent(int amount)
    {
        Amount = amount;
    }
}
public class GearUpgradeQuestEvent : QuestEvent
{
   
    public GearUpgradeQuestEvent()
    {
        
    }
}
public class CurrencyQuestEvent : QuestEvent
{
    public int Amount;
    public Currency Currency;
    public CurrencyQuestEvent(Currency currency,int amount)
    {
        Amount = amount;
        Currency = currency;
    }
}
public class ObtainGearQuestEvent : QuestEvent
{
    public GearType? Type;
    public ObtainGearQuestEvent(GearType? type)
    {
        Type = type;
    }
}