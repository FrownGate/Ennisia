public abstract class Entity
{
    protected internal float Level { get; protected set; }
    protected internal float MaxHp { get; protected set; }
    protected internal float Damage { get; protected set; }
    protected internal float Speed { get; protected set; }
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