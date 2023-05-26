public abstract class Entity
{
    protected internal float Level { get;  set; }
    protected internal float MaxHp { get;  set; }
    protected internal float Damage { get;  set; }
    protected internal float PhysicalRatio { get;  set; }
    protected internal float MagicalRatio { get;  set; }
    protected internal float Defense { get;  set; }
    protected internal float PenetrationDefense { get;  set; }
    protected internal float Speed { get;  set; }
    protected internal float CurrentHp { get; set; }
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