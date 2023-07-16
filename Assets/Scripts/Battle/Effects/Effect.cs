using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public EffectSO Data { get; set; }
    public readonly Dictionary<Attribute, ModifierID> Modifiers = new();
    public int Duration { get; set; }
    public int InitialDuration { get; set; }
    public bool HasAlteration => Data.Alteration;
    private bool IsExpired => Duration <= 0;

    public int Stacks { get; protected set; } = 0;
    public bool IsStackable = false;
    protected int _maxStacks;

    public Effect(int? duration = null)
    {
        Data = Resources.Load<EffectSO>("SO/Effects/" + GetType().Name);
        InitialDuration = duration ?? Data.Duration;
        Duration = 0;
    }

    public virtual void AlterationEffect(Entity target) { }
    public virtual float GetMultiplier() { return 1.0f; }

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
        Cleanse(target);
    }

    public void ResetDuration()
    {
        Duration = InitialDuration;
    }

    public void Cleanse(Entity target)
    {
        foreach (var modifier in Modifiers) target.Stats[modifier.Key].RemoveModifier(modifier.Value);
        target.Effects.Remove(this);
    }

    public void ApplyStack(int stack)
    {
        Stacks = Stacks + stack <= _maxStacks ? Stacks += stack : _maxStacks;
    }
}

public class AttackBuff : Effect
{
    public AttackBuff(int? duration = null) : base(duration) { }
}
public class DefenseBuff : Effect
{
    public DefenseBuff(int? duration = null) : base(duration) { }
}
public class CritRateBuff : Effect
{
    public CritRateBuff(int? duration = null) : base(duration) { }
}
public class CritDmgBuff : Effect
{
    public CritDmgBuff(int? duration = null) : base(duration) { }
}
public class BreakAttack : Effect
{
    public BreakAttack(int? duration = null) : base(duration) { }
}
public class BreakDefense : Effect
{
    public BreakDefense(int? duration = null) : base(duration) { }
}
public class Immunity : Effect
{
    public Immunity(int? duration = null) : base(duration) { }
}