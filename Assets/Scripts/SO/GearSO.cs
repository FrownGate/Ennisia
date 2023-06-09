using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGear", menuName = "Ennisia/Gear")]
public class GearSO : ScriptableObject
{
    public int Id;
    public string Name;
    public int Level = 1;
    public Item.GearType Type;
    public Item.ItemRarity Rarity;
    public Item.AttributeStat Attribute;
    public float StatValue;
    public string Description;
    //public Sprite Icon;
    public Dictionary<Item.AttributeStat, float> Substats;

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

    public void Unequip()
    {
        Id = 0; //If Id = 0 then no gear is equipped
    }
}