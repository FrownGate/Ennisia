﻿using System.Collections.Generic;

public abstract class Entity
{
    protected internal int Level { get;  set; }
    protected internal float MaxHp { get;  set; }
    protected internal float Attack { get;  set; }
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

    //protected internal List<Debuff> DebuffsList

    protected internal List<Skill> Skills { get; protected set; }
    public bool IsSelected { get; protected set; } = false;

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

    public virtual bool HaveBeTargeted()
    {
        return true;
    }
    
    public virtual void ResetTargetedState()
    {
        return;
    }
}