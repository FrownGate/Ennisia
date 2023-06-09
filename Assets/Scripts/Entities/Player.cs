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

        Weapon = Resources.Load<GearSO>("SO/EquippedGears/Weapon");
        Weapon.Init();

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
            Weapon.FirstSkill,
            Weapon.SecondSkill
        };
    }
}