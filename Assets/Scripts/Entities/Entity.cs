using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public float CurrentHp { get; set; }
    public Dictionary<Item.AttributeStat, Stat<float>> Stats { get; private set; }
    public GearSO Weapon { get; set; }
    public List<Skill> Skills { get; protected set; }
    public List<BuffEffect> Buffs { get; protected set; } = new();
    public List<BuffEffect> Alterations { get; protected set; } = new();
    public bool IsSelected { get; protected set; } = false;
    
    public bool IsDead
    {
        get => CurrentHp <= 0;
        private set { }
    }

    //TODO -> replace and remove all following
    public float MaxHp { get; set; }
    public float Attack { get; set; }
    public float PhysAtk { get; set; }
    public float MagicAtk { get; set; }
    public float PhysDef { get; set; }
    public float MagicDef { get; set; }
    public float CritRate { get; set; }
    public float CritDamage { get; set; }
    public float DefIgnored { get; set; }
    public float Shield { get; set; }
    public float Speed { get; set; }
        
    public int atkBar { get; set; }
    public int atkBarFillAmount { get; set; }
    public int atkBarPercentage { get; set; }

    public Entity()
    {
        //TODO -> Use CSV to set all values
        Stats = new();

        foreach (string stat in Enum.GetNames(typeof(Item.AttributeStat)))
        {
            Stats[Enum.Parse<Item.AttributeStat>(stat)] = new(10);
        }

        //TODO -> Take off shield from enum in item
    }

    public virtual void TakeDamage(float damage)
    {
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
        }
        
    }

    public virtual bool HaveBeenTargeted() { return true; }
    public virtual void ResetTargetedState() { }
    public virtual void HaveBeenSelected() { }
    
    public ModifierID AlterateStat(Item.AttributeStat stat, Func<float, float> func, int layer = 1)
    {
        return Stats[stat].AddModifier(func, layer);
    }
    public void AddBuffEffect(BuffEffect buff)
    {
        //var existingBuff = Buffs.FirstOrDefault(st => st.ModifiedStats.ToString() == buff.ModifiedStats.ToString());
        if (Buffs.Contains(buff))
        {
            return;
        }
        else
        {
            Buffs.Add(buff);
        }
    }
    public void AddAlteration(BuffEffect alteration)
    {
        var existingAlt = Alterations.FirstOrDefault(a => a.State.ToString() == alteration.State.ToString());
        if (existingAlt != null)
        {
            existingAlt.ResetDuration();
        }
        else
        {
            Alterations.Add(alteration);
        }
    }
    
}