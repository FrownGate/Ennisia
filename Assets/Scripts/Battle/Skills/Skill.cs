using System;
using System.Collections.Generic;
using UnityEngine;
using CheatCodeNS;

public abstract class Skill
{
    public static event Action OnLevelUp;

    public SkillHUD Button { get; set; }

    public SkillSO Data { get; protected set; }
    public float RatioModifier { get; protected set; }
    public float DamageModifier { get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public int Cooldown { get; set; }
    public int Level { get; set; }
    public float StatUpgrade1 { get; set; }
    public float StatUpgrade2 { get; set; }
    public float TotalDamage { get; set; }
    private float _ratio;
    private float _defense;
    //TODO -> Move fields/functions in child classes

    protected Dictionary<Attribute, ModifierID> _modifiers;

    public Skill()
    {
        Level = 0; //TODO -> Get from database
        var skillTypeValues = Enum.GetValues(typeof(SkillType));

        //TODO -> optimization may be needed -> all SO are loaded for each skill ?
        foreach (SkillType folderType in skillTypeValues)
        {
            var skillSOs = Resources.LoadAll<SkillSO>($"SO/Skills/{folderType}");

            foreach (var skillSO in skillSOs)
            {
                if (skillSO.name != GetType().Name) continue;
                Data = Resources.Load<SkillSO>($"SO/Skills/{folderType}/" + GetType().Name);
                break;
            }

            if (Data != null)
            {
                break;
            }
        }
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity caster, int turn, List<Entity> allies) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies) { }
    public virtual float SkillBeforeUse(List<Entity> targets, Entity caster, int turn, List<Entity> allies) { return 0; }
    public virtual float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies) { return 0; }
    public virtual void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken,
        List<Entity> allies) { }
    public virtual float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage,
        List<Entity> allies) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity caster, int turn, float damage,
        List<Entity> allies) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage,
        List<Entity> allies) { }
    public void Upgrade(int _Level) { Level = _Level; }

    public void Upgrade()
    {
        if (Level >= 5) return;
        Debug.Log($"Upgrading {Data.Name} skill...");

        Level++;
        OnLevelUp?.Invoke();
    }

    public virtual float DamageCalculation(Entity target, Entity caster)
    {
        _ratio = Data.IsMagic ? caster.Stats[Attribute.MagicalDamages].Value : caster.Stats[Attribute.PhysicalDamages].Value;
        _defense = Data.IsMagic ? target.Stats[Attribute.MagicalDefense].Value : target.Stats[Attribute.PhysicalDefense].Value;
        float dmgReduction = _defense / (_defense + 1000);
        float damage = caster.Stats[Attribute.Attack].Value * (Data.DamageRatio / 100 + RatioModifier) * (1+_ratio/100) * (1-dmgReduction);
        foreach (var effect in target.Effects)
        {
            damage *= effect.GetMultiplier();
        }
        return damage;
    }

    public void TakeOffStats(Entity caster)
    {
        foreach (var modifier in _modifiers) caster.Stats[modifier.Key].RemoveModifier(modifier.Value);
    }

    public void Tick()
    {
        if (CheatCodeManager.Lazy.Value.ActiveCheatCodes.Contains(CheatCode.NoCooldown))
        {
            Cooldown = 1;
        }

        Cooldown = Cooldown > 0 ? Cooldown - 1 : 0;
        if (Button != null) Button.ToggleUse(Cooldown == 0);
    }

    public void ResetCoolDown(int duration)
    {
        Cooldown = duration;
    }
}
