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

    //public override void TakeDamage(float damage)
    //{
    //    Debug.Log($"Damage taken : {damage}");
    //    CurrentHp -= damage;
    //    //IsSelected = false;
    //}

    public override bool HaveBeenTargeted()
    {
        return IsSelected && !IsDead;
    }
}