using System.Collections.Generic;

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

    //protected internal List<Debuff> DebuffsList
    protected internal WeaponSO WeaponSO { get; set; }
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
}