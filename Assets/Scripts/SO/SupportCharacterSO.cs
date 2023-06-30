using System;
using UnityEngine;
using NaughtyAttributes;

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
    [Expandable] public SkillSO PrimarySkillData;
    [Expandable] public SkillSO SecondarySkillData;
    [HideInInspector] public Skill PrimarySkill;
    [HideInInspector] public Skill SecondarySkill;
    public string Description;
    public string Catchphrase;
    //public Sprite Icon;

    public void Init()
    {
        Type type = Type.GetType(CSVUtils.GetFileName(PrimarySkillData.Name));
        PrimarySkill = (Skill)Activator.CreateInstance(type);
        type = Type.GetType(CSVUtils.GetFileName(SecondarySkillData.Name));
        SecondarySkill = (Skill)Activator.CreateInstance(type);
    }
}