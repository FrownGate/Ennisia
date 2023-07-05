using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    public static event Action OnLevelUp;

    public Button SkillButton { get; set; }

    public SkillSO Data { get; protected set; }
    public float DamageModifier {  get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public float Cooldown { get; set; }
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
        Data = Resources.Load<SkillSO>("SO/Skills/" + GetType().Name);
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity caster, int turn) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn) { }
    public virtual float SkillBeforeUse(List<Entity> targets, Entity caster, int turn) { return 0; }
    public virtual float Use(List<Entity> targets, Entity caster, int turn) { return 0; }
    public virtual void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken) { }
    public virtual float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity caster, int turn, float damage) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage) { }
    public virtual float DamageCalculation(Entity target, Entity caster)
    {
        _ratio = Data.IsMagic ? caster.Stats[Attribute.MagicalDamages].Value : caster.Stats[Attribute.PhysicalDamages].Value;
        _defense = Data.IsMagic ? target.Stats[Attribute.MagicalDefense].Value : target.Stats[Attribute.PhysicalDefense].Value;
        float damage = (caster.Stats[Attribute.Attack].Value * Data.DamageAmount * _ratio * ((1000 - (_defense - target.Stats[Attribute.DefIgnored].Value)) / 1000));
        return damage;
    }
    
    public void TakeOffStats(Entity caster)
    {
        foreach (var modifier in _modifiers) caster.Stats[modifier.Key].RemoveModifier(modifier.Value);
    }

    public virtual void Upgrade()
    {
        if (Level >= 5) return;
        Debug.Log($"Upgrading {Data.Name} skill...");

        Level++;
        OnLevelUp?.Invoke();
    }

    public virtual void Upgrade(int _Level) { Level = _Level; }

    public void Tick()
    {
        Cooldown = Cooldown > 0 ? Cooldown - 1 : 0;
        if (SkillButton != null) SkillButton.interactable = Cooldown == 0;
    }

    public void ResetCoolDown(int duration)
    {
        Cooldown = duration;
    }
}