using System;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute //TODO -> move DefIgnored alone
{
    HP, Attack, PhysicalDamages, MagicalDamages, PhysicalDefense, MagicalDefense, CritRate, CritDmg, Speed, DefIgnored
}

public abstract class Entity
{
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual int Level { get; set; }
    public virtual Dictionary<Attribute, Stat<float>> Stats { get; private set; }
    public virtual GearSO Weapon { get; set; }

    public int Id { get; set; }

    public float CurrentHp { get; set; }
    public float DefIgnored { get; set; }
    public float Shield { get; set; }

    public List<Skill> Skills { get; protected set; }
    public List<Effect> Effects { get; protected set; } = new();
    public bool IsSelected { get; protected set; } = false;
    public bool IsDead => CurrentHp <= 0;

    public int AtkBar { get; set; }
    public int AtkBarFillAmount { get; set; }
    public int AtkBarPercentage { get; set; }

    public EntityHUD HUD { get; set; }

    public Entity(Dictionary<Attribute, float> stats = null)
    {
        //TODO -> Use CSV to set all values
        Stats = stats != null ? CustomStats(stats) : DefaultStats();

        CurrentHp = Stats[Attribute.HP].Value;
        Debug.Log($"current hp : {CurrentHp}");

        //Testing effects
        //Debug.Log(Stats[AttributeStat.Attack].Value);
        //ApplyEffect(new AttackBuff());
        //Debug.Log(Stats[AttributeStat.Attack].Value);
    }

    private Dictionary<Attribute, Stat<float>> CustomStats(Dictionary<Attribute, float> stats)
    {
        Dictionary<Attribute, Stat<float>> dictionary = new();

        foreach (var stat in stats) dictionary[stat.Key] = new(stat.Value);

        return dictionary;
    }

    protected Dictionary<Attribute, Stat<float>> DefaultStats()
    {
        Dictionary<Attribute, Stat<float>> dictionary = new();

        foreach (string stat in Enum.GetNames(typeof(Attribute)))
        {
            dictionary[Enum.Parse<Attribute>(stat)] = new(1);
        }

        return dictionary;
    }

    public void TakeDamage(float damage)
    {
        if (this is Player && CheatCodeManager.Lazy.Value.ActiveCheatCodes.Contains(CheatCode.Unkillable))
        {
            return;
        }
        Debug.Log($"Damage taken : {damage}");
        if (Shield > 0)
        {
            Shield -= damage;

            if (damage > Shield)
            {
                Shield = 0;
                CurrentHp -= damage - Shield;
            }
        }
        else
        {
            CurrentHp -= damage;
            CurrentHp = CurrentHp < 0 ? 0 : CurrentHp;
        }
    }

    public virtual void Heal(float amount)
    {
        CurrentHp += amount;
        CurrentHp = CurrentHp > Stats[Attribute.HP].Value ? Stats[Attribute.HP].Value : CurrentHp;
    }

    public virtual bool HaveBeenTargeted() { return true; }
    public virtual void ResetTargetedState() { IsSelected = false; }
    public virtual void HaveBeenSelected() { IsSelected = true; }

    //public ModifierID AlterateStat(AttributeStat stat, Func<float, float> func, int layer = 1)
    //{
    //    return Stats[stat].AddModifier(func, layer);
    //}

    public void ApplyEffect(Effect effect)
    {
        Effect existingEffect = Effects.Find(x => x.Data.Name == effect.Data.Name);

        if (existingEffect != null)
        {
            existingEffect.ResetDuration();
            return;
        }

        Effects.Add(effect);
        if (!effect.HasAlteration) effect.AddEffectModifiers(this);
    }

    public void Cleanse()
    {
        foreach (var effect in Effects)
        {
            if (!effect.HasAlteration) effect.Cleanse(this);
        }
    }

    public void Strip()
    {
        foreach (var effect in Effects)
        {
            if (effect.HasAlteration) effect.Cleanse(this);
        }
    }

    public void ResetAtb()
    {
        AtkBar = 0;
        AtkBarPercentage = 0;
    }

    public bool HasBuff()
    {
        foreach (var effect in Effects)
        {
            if (!effect.HasAlteration) return true;
        }

        return false;
    }

    public bool HasAlteration()
    {
        foreach (var effect in Effects)
        {
            if (effect.HasAlteration) return true;
        }

        return false;
    }
}