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
        // assign all parameters to the enemy
        Id = id;
        Name = name;
        Description = description;
        Level = 1;

        for (int i = 0; i < stats.Length; i++)
        {
            switch (stats[i])
            {
                case "Hp":
                    MaxHp = statNumbers[i];
                    CurrentHp = MaxHp;
                    break;
                case "Atk":
                    Attack = statNumbers[i];
                    break;
                case "Phys Atk":
                    PhysAtk = statNumbers[i];
                    break;
                case "Phys Def":
                    PhysDef = statNumbers[i];
                    break;
                case "Magic Atk":
                    MagicAtk = statNumbers[i];
                    break;
                case "Magic Def":
                    MagicDef = statNumbers[i];
                    break;
                case "Crit Rate":
                    CritRate = statNumbers[i];
                    break;
                case "Crit Damage":
                    CritDamage = statNumbers[i];
                    break;
                case "SPD":
                    Speed = statNumbers[i];
                    break;
                default:
                    break;
            }
        }


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