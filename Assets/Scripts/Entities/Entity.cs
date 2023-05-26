using System.Collections.Generic;

public abstract class Entity
{
    protected internal int Level { get; protected set; }
    protected internal float MaxHp { get; protected set; }
    protected internal float Attack { get; protected set; }
    protected internal float PhysAtk { get; protected set; }
    protected internal float MagicAtk { get; protected set; }
    protected internal float PhysDef { get; protected set; }
    protected internal float MagicDef { get; protected set; }
    protected internal float CritRate { get; protected set; }
    protected internal float CritDamage { get; protected set; }
    protected internal float DefIgnored { get; protected set; }
    protected internal float Shield { get; protected set; }
    protected internal float Speed { get; protected set; }
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
}