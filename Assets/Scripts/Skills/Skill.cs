using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public SkillSO Data { get; protected set; }
    public float DamageModifier {  get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public float Cooldown { get; set; }
    public string FileName { get; private set; }

    public Skill()
    {
        Debug.Log("creating skill");
        FileName = GetType().Name;
        Data = Resources.Load<SkillSO>("SO/Skills/" + FileName);
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public virtual float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void TakeOffStats(List<Entity> targets, Entity player, int turn) { }
}