using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New Data")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public string weaponType;
    public bool isMagic;
    public string statName;
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
            statValue += (statUpgrade * level) + (statValue * ratioUpgrade);
        }
        else { }
    }

    public void Upgrade(int _level)
    {
        if(_level <= 50)
        {
            statValue += (statUpgrade * _level) + (statValue * ratioUpgrade);
        }
        else { } 
    }

}
