using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Ennisia/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string Name;
    public Item.GearType Type;
    public Item.AttributeStat Stat;
    public bool IsMagic;
    public float StatValue; 
    public float StatUpgrade;
    public float RatioUpgrade;
    public int Level = 1;
    public SkillSO FirstSkillData;
    public SkillSO SecondSkillData;
    [HideInInspector] public Skill FirstSkill;
    [HideInInspector] public Skill SecondSkill;

    //TODO -> Move Level and Upgrade system to Gear class

    public void Init()
    {
        Type type = System.Type.GetType(CSVUtils.GetFileName(FirstSkillData.Name));
        FirstSkill = (Skill)Activator.CreateInstance(type);
        type = System.Type.GetType(CSVUtils.GetFileName(SecondSkillData.Name));
        SecondSkill = (Skill)Activator.CreateInstance(type);
    }

    public void Upgrade()
    {
        if (Level <= 50)
        {
            StatValue += (StatUpgrade * Level) + (StatValue * RatioUpgrade * Level);
        }
    }

    public void Upgrade(int level)
    {
        if (level <= 50)
        {
            StatValue += (StatUpgrade * level) + (StatValue * RatioUpgrade * level);
        }
    }
}