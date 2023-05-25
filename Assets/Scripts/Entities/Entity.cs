public abstract class Entity
{
    protected internal int Level { get; protected set; }
    protected internal int MaxHp { get; protected set; }
    protected internal int Damage { get; protected set; }
    protected internal int Speed { get; protected set; }
    protected internal int CurrentHp { get; set; }
    public bool IsSelected { get; protected set; } = false;

    public bool IsDead
    {
        get
        {
            return CurrentHp <= 0;
        }
        private set { }
    }

    public virtual void TakeDamage(int damage)
    {
        CurrentHp -= damage;
    }

    public virtual bool HaveBeTargeted()
    {
        return true;
    }
}