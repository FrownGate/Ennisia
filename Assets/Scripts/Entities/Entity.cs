using System.Collections.Generic;
using System.Linq;

public abstract class Entity
{
    protected internal int Id { get; set; }
    protected internal string Name { get; set; }
    protected internal string Description { get; set; }
    protected internal int Level { get; set; }
    protected internal float MaxHp { get; set; }
    protected internal float Attack { get; set; }
    protected internal float PhysAtk { get; set; }
    protected internal float MagicAtk { get; set; }
    protected internal float PhysDef { get; set; }
    protected internal float MagicDef { get; set; }
    protected internal float CritRate { get; set; }
    protected internal float CritDamage { get; set; }
    protected internal float DefIgnored { get; set; }
    protected internal float Shield { get; set; }
    protected internal float Speed { get; set; }
    protected internal float CurrentHp { get; set; }
    
    private readonly Dictionary<string, float> _baseValues;

    //protected internal List<Debuff> DebuffsList
    protected internal GearSO Weapon { get; set; }
    protected internal List<Effect> EffectList{ get; protected set; }
    
    protected internal GearSO WeaponSO { get; set; }
    protected internal List<Skill> Skills { get; protected set; }
    public bool IsSelected { get; protected set; } = false;
    
    public Entity()
    {
        _baseValues = new Dictionary<string, float>();
        EffectList = new List<Effect>();
        StoreBaseValues();
    }

    public bool IsDead
    {
        get
        {
            return CurrentHp <= 0;
        }
        private set { }
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }

    public virtual bool HaveBeenTargeted() { return true; }
    public virtual void ResetTargetedState() { }
    public virtual void HaveBeenSelected() { }

    public virtual Dictionary<string, int> GetAllStats()
    {
        Dictionary<string, int> stats = new()
        {
            { "MaxHp", (int)MaxHp },
            { "Atk", (int)Attack },
            { "PhysAtk", (int)PhysAtk },
            { "PhysDef", (int)PhysDef },
            { "MagicAtk", (int)MagicAtk },
            { "MagicDef", (int)MagicDef },
            { "CritRate", (int)CritRate },
            { "CritDamage", (int)CritDamage },
            { "Speed", (int)Speed },
            // { "CurrentHp", (int)CurrentHp },
            // { "Shield", (int)Shield },
            // { "DefIgnored", (int)DefIgnored },
        };

        return stats;
    }
    
    public void ApplyEffect(Effect effectToApply)
    {
        var existingEffect = EffectList.FirstOrDefault(e => e.GetType() == effectToApply.GetType());
        
        if (existingEffect != null)
        {
            existingEffect.ResetDuration(effectToApply.Duration);
        }
        else
        {
            EffectList.Add(effectToApply);
        }
    }

    public void RemoveEffect(Effect effectToRemove)
    {
        EffectList.Remove(effectToRemove);
        
        foreach (var stat in effectToRemove.ModifiedStats)
        {
            ResetValueToBase(stat);
        }
        
    }
    
    private void StoreBaseValues()
    {
        _baseValues["MaxHp"] = MaxHp;
        _baseValues["Atk"] = Attack;
        _baseValues["PhysAtk"] = PhysAtk;
        _baseValues["PhysDef"] = PhysDef;
        _baseValues["MagicAtk"] = MagicAtk;
        _baseValues["MagicDef"] = MagicDef;
        _baseValues["CritRate"] = CritRate;
        _baseValues["CritDamage"] = CritDamage;
        _baseValues["Speed"] = Speed;
    }

    private void ResetValueToBase(string valueName)
    {
        if (!_baseValues.ContainsKey(valueName)) return;
        switch (valueName)
        {
            case "MaxHp":
                MaxHp = _baseValues[valueName];
                break;
            case "Attack":
                Attack = _baseValues[valueName];
                break;
            case "PhysAtk":
                PhysAtk = _baseValues[valueName];
                break;
            case "MagicAtk":
                MagicAtk = _baseValues[valueName];
                break;
            case "PhysDef":
                PhysDef = _baseValues[valueName];
                break;
            case "MagicDef":
                MagicDef = _baseValues[valueName];
                break;
            case "CritRate":
                CritRate = _baseValues[valueName];
                break;
            case "CritDamage":
                CritDamage = _baseValues[valueName];
                break;
            case "Speed":
                Speed = _baseValues[valueName];
                break;
        }

    }
    
    
}