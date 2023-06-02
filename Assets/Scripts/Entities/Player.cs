using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Player()
    {
        MaxHp = 300;
        Attack = 15;
        Speed = 30000;
        CurrentHp = MaxHp / 2;
        WeaponSO = Resources.Load<WeaponSO>("SO/Weapon"); //TODO-> get Equipped Weapon
        WeaponSO.Init();
        GetSkill();
    }

    private void GetSkill()
    {
        Skills = new List<Skill>
        {
            new Bonk(),
            (Skill)Activator.CreateInstance(Type.GetType(WeaponSO.skillName1)),
            (Skill)Activator.CreateInstance(Type.GetType(WeaponSO.skillName1)),
        };
        
    }
}