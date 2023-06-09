using UnityEngine;

public class Player : Entity
{
    public Player()
    {
        //TODO -> Set stats with CSV or another method
        MaxHp = 300;
        Attack = 15;
        Speed = 30000;
        CurrentHp = MaxHp / 2;

        Weapon = Resources.Load<GearSO>("SO/EquippedGears/Weapon");
        Weapon.Init();

        InitSkills();
    }

    private void InitSkills()
    {
        Skills = new()
        {
            new Bonk(),
            Weapon.FirstSkill,
            Weapon.SecondSkill
        };
    }
}