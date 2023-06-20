using System;
using System.Collections.Generic;

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
            Stats[Enum.Parse<Item.AttributeStat>(stat)] = new(1);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }

    public virtual bool HaveBeenTargeted() { return true; }
    public virtual void ResetTargetedState() { }
    public virtual void HaveBeenSelected() { }
    
    public Stat<float>.ModifierID AlterateStat(Item.AttributeStat stat, Func<float, float> func, int layer = 1)
    {
        return Stats[stat].AddModifier(func, layer);
    }
}