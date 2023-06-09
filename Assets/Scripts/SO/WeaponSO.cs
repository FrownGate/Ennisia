using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Ennisia/Weapon")]
public class WeaponSO : GearSO
{
    //Weapons
    public bool IsMagic;
    public SkillSO FirstSkillData;
    public SkillSO SecondSkillData;
    [HideInInspector] public Skill FirstSkill;
    [HideInInspector] public Skill SecondSkill;

    public void Init()
    {
        Type type = System.Type.GetType(CSVUtils.GetFileName(FirstSkillData.Name));
        FirstSkill = (Skill)Activator.CreateInstance(type);
        type = System.Type.GetType(CSVUtils.GetFileName(SecondSkillData.Name));
        SecondSkill = (Skill)Activator.CreateInstance(type);
    }
}
