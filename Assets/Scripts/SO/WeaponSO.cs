using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New Data")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType
    {
        SWORD = 0,
        STAFF = 1,
        SCHYTHE = 2,
        DAGGERS = 3,
        HAMMER = 4,
        SHIELD = 5,
        BOW = 6,
    }
    public enum StatType
    {
        PHYSICAL = 0,
        MAGICAL = 1,
        ATTACK = 2,
        HEALTH = 3,
        DEFENSE = 4,
        CRITICALRATE = 5,
        CRITICALDAMAGE = 6,
        SPEED = 7,
    }

    public string weaponName;
    public WeaponType weaponType;
    public bool isMagic;
    public StatType statType;
    public float statValue; 
    public float statUpgrade;
    public float ratioUpgrade;
    public int level;
    public string skillName1;
    public string skillName2;
    Type _type1;
    Type _type2;
    [HideInInspector] public Skill _skill1;
    [HideInInspector] public Skill _skill2;
    private void Awake()
    {
        _type1 = Type.GetType(skillName1);
        _skill1 = (Skill)Activator.CreateInstance(_type1);
        _type2 = Type.GetType(skillName2);
        _skill2 = (Skill)Activator.CreateInstance(_type2);
    }

    public void Upgrade()
    {
        if (level <= 50)
        {
            statValue += (statUpgrade * level) + (statValue * ratioUpgrade * level);
        }
        else { }
    }

    public void Upgrade(int _level)
    {
        if(_level <= 50)
        {
            statValue += (statUpgrade * _level) + (statValue * ratioUpgrade * _level);
        }
        else { } 
    }

}
