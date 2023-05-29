using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    public SkillData Data { get; protected set; }
    public float DamageModifier {  get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public float Cooldown { get; protected set; }
    public string FileName { get; protected set; }

    private void Start()
    {
        FileName = GetType().Name;
        Data = Resources.Load<SkillData>("SO/Skills/" + FileName + ".asset");
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public virtual float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
}
