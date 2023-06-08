using System.Collections.Generic;
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

        WeaponSO = Resources.Load<WeaponSO>("SO/EquippedGears/Weapon");
        WeaponSO.Init();

        InitSkills();
    }

    public Player(Dictionary<string, float> stats)
    {
        stats = new Dictionary<string, float>()
        {
            { "MaxHp", (int)MaxHp },
            { "Atk", (int)Attack },
            { "PhysAtk", (int)PhysAtk },
            { "PhysDef", (int)PhysDef },
            { "MagicAtk", (int)MagicAtk },
            { "MagicDef", (int)MagicDef },
            { "CritRate", (int)CritRate },
            { "CritDamage", (int)CritDamage },
            { "Speed", (int)Speed },
        };
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