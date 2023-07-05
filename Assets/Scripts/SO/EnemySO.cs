using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Ennisia/Enemy")]
public class EnemySO : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public SerializedDictionary<Attribute, float> Stats = new();
    [Expandable] public List<SkillSO> SkillsData = new();
    [HideInInspector] public List<Skill> Skills = new();

    public void Init()
    {
        foreach (var type in SkillsData.Select(skillData => Type.GetType(CSVUtils.GetFileName(skillData.Name))))
        {
            Skills.Add((Skill)Activator.CreateInstance(type));
        }
    }
}
