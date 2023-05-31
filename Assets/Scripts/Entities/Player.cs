using System.Collections.Generic;

public class Player : Entity
{
    public Player()
    {
        MaxHp = 300;
        Attack = 15;
        Speed = 30000;
        CurrentHp = MaxHp / 2;
        GetSkill();
    }

    private void GetSkill()
    {
        Skills = new List<Skill>
        {
            new Bonk(),
            WeaponSO._skill1,
            WeaponSO._skill2
        };

    }
}