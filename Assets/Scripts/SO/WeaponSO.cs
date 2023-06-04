using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Ennisia/Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType //TODO -> move elsewhere for better maintenance
    {
        Sword, Staff, Scythe, Daggers, Hammer, Shield, Bow
    }

    public enum StatType //TODO -> move elsewhere for better maintenance
    {
        Physical, Magical, Attack, Health, Defense, CriticalRate, CriticalDamage, Speed
    }

    public string Name;
    public WeaponType Type;
    public bool IsMagic;
    public StatType Stat;
    public float StatValue; 
    public float StatUpgrade;
    public float RatioUpgrade;
    public int Level;
    public SkillSO FirstSkillData;
    public SkillSO SecondSkillData;
    [HideInInspector] public Skill FirstSkill;
    [HideInInspector] public Skill SecondSkill;

    //TODO -> Move Level and Upgrade system to Gear class

    public void Init()
    {
        Type type = System.Type.GetType(FirstSkillData.Name);
        FirstSkill = (Skill)Activator.CreateInstance(type);
        type = System.Type.GetType(SecondSkillData.Name);
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