using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewGear", menuName = "Ennisia/Gear")]
public class GearSO : ScriptableObject
{
    public int Id;
    public string Name;
    public int Level = 1;
    public Item.GearType Type;
    public Item.ItemRarity Rarity;
    public Attribute Attribute;
    public float StatValue;
    public string Description;
    //public Sprite Icon;
    public Dictionary<Attribute, float> Substats;

    //Weapons
    public Item.GearWeaponType WeaponType;
    public bool IsMagic;
    [Expandable] public SkillSO FirstSkillData;
    [Expandable] public SkillSO SecondSkillData;
    [HideInInspector] public Skill FirstSkill;
    [HideInInspector] public Skill SecondSkill;

    public void Init()
    {
        Type type = System.Type.GetType(CSVUtils.GetFileName(FirstSkillData.Name));
        FirstSkill = (Skill)Activator.CreateInstance(type);
        type = System.Type.GetType(CSVUtils.GetFileName(SecondSkillData.Name));
        SecondSkill = (Skill)Activator.CreateInstance(type);
    }

    public void Unequip()
    {
        Id = 0; //If Id = 0 then no gear is equipped
    }
}