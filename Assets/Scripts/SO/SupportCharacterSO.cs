using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "New Support", menuName = "Ennisia/Support")]
public class SupportCharacterSO : ScriptableObject
{
    public int Id = 1;
    public string Name;
    public string Rarity;
    public string Race;
    public string Job;
    public Element.ElementType Element;
    //TODO -> Use array or list to store supports skills
    [Expandable] public List<SkillSO> SkillsData = new();
    [HideInInspector] public List<Skill> Skills = new();
    public string Description;
    public string Catchphrase;
    //public Sprite Icon;

    public void Init()
    {
        foreach (var type in SkillsData.Select(skillData => Type.GetType(CSVUtils.GetFileName(skillData.Name))))
        {
            Skills.Add((Skill)Activator.CreateInstance(type));
        }
    }
}