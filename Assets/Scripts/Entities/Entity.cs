using System;
using System.Collections.Generic;
using UnityEngine;
using CheatCodeNS;

public enum Attribute //TODO -> move DefIgnored alone
{
    HP, Attack, PhysicalDamages, MagicalDamages, PhysicalDefense, MagicalDefense, CritRate, CritDmg, Speed, DefIgnored
}

public abstract class Entity
{
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual int Level { get; set; }
    public virtual Dictionary<Attribute, Stat<float>> Stats { get; set; }

    //Player Datas
    public virtual GearSO Weapon { get; set; }
    public virtual SupportCharacterSO[] EquippedSupports { get; set; }
    public virtual Dictionary<GearType, Gear> EquippedGears { get; set; }

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

    public float AmountHealed { get; set; }
    public bool Healed { get; set; }
    public EntityHUD HUD { get; set; }

    public Entity(Dictionary<Attribute, float> stats = null)
    {
        //TODO -> Use CSV to set all values
        Stats = stats != null ? CustomStats(stats) : DefaultStats();

        CurrentHp = Stats[Attribute.HP].Value;

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
        Healed = true;
        AmountHealed = amount;
        CurrentHp += amount;
        CurrentHp = CurrentHp > Stats[Attribute.HP].Value ? Stats[Attribute.HP].Value : CurrentHp;
    }

    public void resetHealed() => Healed = false;


    public virtual bool HaveBeenTargeted() { return true; }
    public virtual void ResetTargetedState() { IsSelected = false; }
    public virtual void HaveBeenSelected() { IsSelected = true; }

    public void ApplyEffect(Effect effect, int stacks = 1)
    {
        if (effect.HasAlteration)
        {
            if (Effects.Find(effect => effect.GetType() == typeof(Immunity)) != null) return;
        }

        Effect existingEffect = Effects.Find(x => x.Data.Name == effect.Data.Name);

        if (existingEffect != null)
        {
            if (existingEffect.IsStackable)
            {
                existingEffect.ApplyStack(stacks);
            }
            existingEffect.ResetDuration();
            return;
        }

        Effects.Add(effect);
        if (effect.IsStackable) effect.ApplyStack(stacks);
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

    public void InitElement()
    {
        List<ElementType> elements = new();

        foreach (SupportCharacterSO support in EquippedSupports)
        {
            if (support == null) return;
            elements.Add(support.Element);
        }

        if (elements[0] != elements[1]) return;
        Type type = Type.GetType(elements[0].ToString());
        Element elementToUse = (Element)Activator.CreateInstance(type);
        elementToUse.Init(this);

        Debug.Log($"{elementToUse.Name} initiated !");
    }
}