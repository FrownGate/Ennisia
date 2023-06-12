using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static UnityEngine.Rendering.DebugUI;

public abstract class Skill
{
    public static event Action LevelUp;
    public SkillSO Data { get; protected set; }
    public float DamageModifier {  get; protected set; }
    public float ShieldModifier { get; protected set; }
    public float HealingModifier { get; protected set; }
    public float Cooldown { get; set; }
    public int Level { get; set; }
    public float StatUpgrade1 { get; set; }
    public float StatUpgrade2 { get; set; }
    public string FileName { get; protected set; }

    public UnityEngine.UI.Button SkillButton { get; set; }

    public Skill()
    {
        Level = 0;
        FileName = GetType().Name;
        Data = Resources.Load<SkillSO>("SO/Skills/" + FileName);
    }

    public virtual void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public virtual float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public virtual void UseIfAttacked(List<Entity> targets, Entity player, int turn) { }
    public virtual float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public virtual void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
    public virtual void TakeOffStats(List<Entity> targets, Entity player, int turn) { }

    public virtual void Upgrade()
    {
        if (Level >= 5) return;
        Debug.Log($"Upgrading {FileName}...");

        Level++;
        LevelUp?.Invoke();
    }

    public virtual void Upgrade(int _Level) { Level = _Level; }
    public void Tick()
    {
        Cooldown = Cooldown > 0 ? Cooldown - 1 : 0;
        if (SkillButton != null)
        {
            SkillButton.interactable = Cooldown == 0;
        }
    }

    public void ResetCoolDown(int duration)
    {
        Cooldown = duration;
    }
}