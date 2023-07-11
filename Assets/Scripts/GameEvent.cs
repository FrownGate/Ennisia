using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class KillGameEvent : GameEvent
{
    public string KilledName;

    public KillGameEvent(string name)
    {
        KilledName = name;
    }
}
