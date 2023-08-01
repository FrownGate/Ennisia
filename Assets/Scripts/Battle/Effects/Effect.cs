using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public EffectSO Data { get; set; }
    public readonly Dictionary<Attribute, ModifierID> Modifiers = new();
    public int Duration { get; set; }
    public int InitialDuration { get; set; }
    public bool HasAlteration => Data.Alteration;
    public bool IsExpired => Duration <= 0;
    public bool IsUndispellable => Data.Undispellable;
    public Entity Target { get; set; }
    public Entity Caster { get; set; }

    public int Stacks { get; protected set; } = 0;
    public bool IsStackable = false;
    protected int _maxStacks;

    public Effect(int? duration = null, Entity caster = null)
    {
        Data = Resources.Load<EffectSO>("SO/Effects/" + GetType().Name);
        InitialDuration = duration ?? Data.Duration;
        Duration = InitialDuration;
        Caster = caster;
    }

    public virtual void AlterationEffect() { }
    public virtual float GetMultiplier() { return 1.0f; }

    public void AddEffectModifiers()
    {
        foreach (var modifier in Data.StatsModifiers)
        {
            Modifiers[modifier.Key] = Target.Stats[modifier.Key].AddModifier((float value) => value * modifier.Value);
        }
    }

    public void Tick()
    {
        Duration--;
    }

    public void ResetDuration()
    {
        Duration = InitialDuration;
    }

    public void RemoveEffect()
    {
        foreach (var modifier in Modifiers) Target.Stats[modifier.Key].RemoveModifier(modifier.Value);
        Target.Effects.Remove(this);
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
public class Invincibility : Effect
{
    public Invincibility(int? duration = null) : base(duration) { }
}
public class Berserk : Effect
{
    public Berserk(int? duration = null) : base(duration) { }
}
public class Taunt : Effect
{
    public Taunt(int? duration = null, Entity caster = null) : base(duration, caster) { }
}