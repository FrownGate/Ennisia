public class Enemy : Entity
{
    public Enemy()
    {
        Attack = 20;
        MaxHp = 5000;
        Level = 10;
        Speed = 200;
        CurrentHp = MaxHp / 2;
    }
    public Enemy(int id, string name, string[] stats, int[] statNumbers, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        Level = 1;
        MaxHp = 5000;
        Speed = 200;
        CurrentHp = MaxHp / 2;
    }

    public override void HaveBeSelected()
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

    public override void ResetTargetedState()
    {
        IsSelected = false;
    }
    
    
}