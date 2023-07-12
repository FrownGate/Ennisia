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