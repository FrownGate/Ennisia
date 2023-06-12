using System.Collections.Generic;
using System.Linq;

public abstract class Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
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
    public float CurrentHp { get; set; }
    //TODO -> use dictionary with stat enum instead
    
    private readonly Dictionary<string, float> _baseValues;

    //protected internal List<Debuff> DebuffsList
    public GearSO Weapon { get; set; }
    public List<BuffEffect> EffectList{ get; protected set; }
    
    public GearSO WeaponSO { get; set; }
    public List<Skill> Skills { get; protected set; }
    public bool IsSelected { get; protected set; } = false;
    
    public Entity()
    {
        _baseValues = new Dictionary<string, float>();
        EffectList = new List<BuffEffect>();
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
    
    public void ApplyEffect(BuffEffect effectToApply)
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

    public void RemoveEffect(BuffEffect effectToRemove)
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