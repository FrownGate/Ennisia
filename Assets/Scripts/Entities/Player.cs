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

        WeaponSO = Resources.Load<WeaponSO>("SO/Weapon"); //TODO -> get Equipped Weapon
        WeaponSO.Init();

        InitSkills();
    }

    private void InitSkills()
    {
        Skills = new()
        {
            new Bonk(),
            WeaponSO.FirstSkill,
            WeaponSO.SecondSkill
        };
    }
}