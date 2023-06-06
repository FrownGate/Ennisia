using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Skill
{
    /*Weapon weapon;*/
    public SkillData Data { get; protected set; }
    public float DamageModifier {  get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public float Cooldown { get; set; }
    public int Level { get; set; }
    public float StatUpgrade1 { get; set; }
    public float StatUpgrade2 { get; set; }
    public string FileName { get; protected set; }

    public Skill()
    {
        Level = 0;
        FileName = GetType().Name;
        Data = Resources.Load<SkillData>("SO/Skills/" + FileName);
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public virtual float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void TakeOffStats(List<Entity> targets, Entity player, int turn) { }
    public virtual void Upgrade()
    {
        if (Level < 5)
        {
            Level += 1;
        }
        else if (Level > 5)
        {
            Level = 5;
        }
        else if (Level < 0)
        {
            Level = 0;
        }
    }
    public virtual void Upgrade(int _Level) { Level = _Level; }
}
