using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SkillsToInitSO : ScriptableObject
{
    [Expandable] public List<SkillSO> SkillsData = new();
    [HideInInspector] public List<Skill> Skills;

    public void Init()
    {
        Skills = new();

        foreach (SkillSO skill in SkillsData)
        {
            Type type = Type.GetType(CSVUtils.GetFileName(skill.Name));
            Skills.Add((Skill)Activator.CreateInstance(type));
        }
    }
}