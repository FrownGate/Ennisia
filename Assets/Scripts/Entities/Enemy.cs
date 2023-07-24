using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy()
    {
        Name = "Betala";
        Stats[Attribute.Speed] = new(40);
        CurrentHp = Stats[Attribute.HP].Value / 2;
    }

    public Enemy(int id, string name, Dictionary<Attribute, float> stats, string description) : base(stats)
    {
        Id = id;
        Name = name;
        Description = description;
        Level = 1;
    }

    public Enemy(int id, EnemySO data) : base(data.Stats)
    {
        Id = id;
        Name = data.Name;
        Description = data.Description;
        Level = 1; //Temp
        IsBoss = data.IsBoss;
    }

    public override bool HaveBeenTargeted()
    {
        return IsSelected && !IsDead;
    }
}