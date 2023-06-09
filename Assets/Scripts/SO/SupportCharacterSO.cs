using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Support", menuName = "Ennisia/Support")]
public class SupportCharacterSO : ScriptableObject
{
    public int Id = 1;
    public string Name;
    public string Rarity;
    public string Race;
    public string Job;
    public string Element; 
    public SkillSO PrimarySkillData;
    public SkillSO SecondarySkillData;
    [HideInInspector] public Skill PrimarySkill;
    [HideInInspector] public Skill SecondarySkill;
    public string Description;
    public string Catchphrase;
    //public Sprite Icon;

    public void Init()
    {
        Type type = System.Type.GetType(CSVUtils.GetFileName(PrimarySkillData.Name));
        PrimarySkill = (Skill)Activator.CreateInstance(type);
        type = System.Type.GetType(CSVUtils.GetFileName(SecondarySkillData.Name));
        SecondarySkill = (Skill)Activator.CreateInstance(type);
    }
}