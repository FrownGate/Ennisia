using System.Collections.Generic;

public class Enemy : Entity
{
    public Enemy()
    {
        /*        Attack = 20;
                MaxHp = 5000;
                Level = 10;
                Speed = 200;*/
        Name = "betala";
        Stats[Item.AttributeStat.Speed] = new(40);
        CurrentHp = Stats[Item.AttributeStat.HP].Value / 2;

    }

    public Enemy(int id, string name, Dictionary<Item.AttributeStat, int> stats, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        Level = 1;

        // assign stats
        foreach (KeyValuePair<Item.AttributeStat, int> stat in stats)
        {
            Stats[stat.Key] = new(stat.Value);
        }
    }

    public override void HaveBeenSelected()
    {
        IsSelected = true;
    }

    public override void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        IsSelected = false;
    }

    public override bool HaveBeenTargeted()
    {
        return IsSelected && !IsDead;
    }

    public override void ResetTargetedState()
    {
        IsSelected = false;
    }
}