using System.Collections.Generic;
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
    public Enemy(int id, string name, Dictionary<string, int> stats, string description)
    {
        // assign all parameters to the enemy
        Id = id;
        Name = name;
        Description = description;
        Level = 1;


        // assign stats
        MaxHp = stats["MaxHp"];
        CurrentHp = MaxHp;
        Attack = stats["Attack"];
        PhysAtk = stats["PhysAtk"];
        PhysDef = stats["PhysDef"];
        MagicAtk = stats["MagicAtk"];
        MagicDef = stats["MagicDef"];
        CritRate = stats["CritRate"];
        CritDamage = stats["CritDamage"];
        Speed = stats["Speed"];



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