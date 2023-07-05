using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGear", menuName = "Ennisia/Gear")]
public class GearSO : ScriptableObject
{
    public int Id;
    public string Name;
    public int Level = 1;
    public GearType Type;
    public Rarity Rarity;
    public Attribute Attribute;
    public float StatValue;
    public string Description;
    //public Sprite Icon;
    public SerializedDictionary<Attribute, float> Substats;

    //Weapons
    public WeaponType WeaponType;
    public bool IsMagic;
    [Expandable] public List<SkillSO> SkillsData = new();
    [HideInInspector] public List<Skill> Skills = new();

    public void Init()
    {
        foreach (var type in SkillsData.Select(skillData => System.Type.GetType(CSVUtils.GetFileName(skillData.Name))))
        {
            Skills.Add((Skill)Activator.CreateInstance(type));
        }
    }

    public void Unequip()
    {
        Id = 0; //If Id = 0 then no gear is equipped
    }
}