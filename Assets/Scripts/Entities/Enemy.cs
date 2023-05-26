public class Enemy : Entity
{
    public Enemy()
    {
        Attack = 20;
        MaxHp = 200;
        Level = 10;
        Speed = 200;
        CurrentHp = MaxHp / 2;
    }

    public void HaveBeSelected()
    {
        IsSelected = true;
    }

    public override void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        IsSelected = false;
    }

    public override bool HaveBeTargeted()
    {
        return IsSelected && !IsDead;
    }
}