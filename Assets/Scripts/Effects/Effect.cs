using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : Effect { }
public class DefenseBuff : Effect { }
public class CritRateBuff : Effect { }
public class CritDmgBuff : Effect { }
public class BreakAttack : Effect { }
public class BreakDefense : Effect { }

public class Effect
{
    public enum AlterationState
    {
        None, Stun, Silence, SupportSilence, DemonicMark
    }

    public EffectSO Data { get; set; }
    public readonly Dictionary<Item.AttributeStat, ModifierID> Modifiers = new();
    public int Duration { get; set; }
    public int InitialDuration { get; set; }
    public bool HasAlteration => Data.Alteration != AlterationState.None;
    private bool IsExpired => Duration <= 0;

    public Effect(int? duration = null)
    {
        Data = Resources.Load<EffectSO>("SO/Effects/" + GetType().Name);
        InitialDuration = duration ?? Data.Duration;
        Duration = 0;
    }

    public virtual void AlterationEffect(Entity target) { }

    public void AddEffectModifiers(Entity target)
    {
        foreach (var modifier in Data.StatsModifiers)
        {
            Modifiers[modifier.Key] = target.Stats[modifier.Key].AddModifier((float value) => value * modifier.Value);
        }
    }

    public void Tick(Entity target)
    {
        Duration--;

        if (!IsExpired) return;

        foreach (var modifier in Modifiers) target.Stats[modifier.Key].RemoveModifier(modifier.Value);
        target.Effects.Remove(this);
    }

    public void ResetDuration()
    {
        Duration = InitialDuration;
    }
}