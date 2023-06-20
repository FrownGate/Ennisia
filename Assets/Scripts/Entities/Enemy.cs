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

    public Enemy(int id, string name, Dictionary<string, int> stats, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        Level = 1;

        
      /*  foreach (KeyValuePair<string, int> stat in stats)
        {
            switch (stat.Key) //TODO -> Use enum instead of strings
            {
                case "Hp":
                    MaxHp = stat.Value;
                    break;
                case "Atk":
                    Attack = stat.Value;
                    break;
                case "PhysAtk":
                    PhysAtk = stat.Value;
                    break;
                case "MagicAtk":
                    MagicAtk = stat.Value;
                    break;
                case "PhysDef":
                    PhysDef = stat.Value;
                    break;
                case "MagicDef":
                    MagicDef = stat.Value;
                    break;
                case "CritRate":
                    CritRate = stat.Value;
                    break;
                case "CritDamage":
                    CritDamage = stat.Value;
                    break;
                case "DefIgnored":
                    DefIgnored = stat.Value;
                    break;
                case "Shield":
                    Shield = stat.Value;
                    break;
                case "Speed":
                    Speed = stat.Value;
                    break;
                default:
                    break;
            }
        }*/
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