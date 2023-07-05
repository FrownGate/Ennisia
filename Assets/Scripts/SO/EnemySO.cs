using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Ennisia/Enemy")]
public class EnemySO : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public SerializedDictionary<Attribute, float> Stats = new();
    [Expandable] public SkillSO Skill1Data;
    [Expandable] public SkillSO Skill2Data;
    [Expandable] public SkillSO Skill3Data;
    [Expandable] public SkillSO PassifData;
    [HideInInspector] public Skill Skill1;
    [HideInInspector] public Skill Skill2;
    [HideInInspector] public Skill Skill3;
    [HideInInspector] public Skill Passif;

    public void Init()
    {
        Type type = Type.GetType(CSVUtils.GetFileName(Skill1Data.Name));
        Skill1 = (Skill)Activator.CreateInstance(type);
        type = Type.GetType(CSVUtils.GetFileName(Skill2Data.Name));
        Skill2 = (Skill)Activator.CreateInstance(type);
        type = Type.GetType(CSVUtils.GetFileName(Skill3Data.Name));
        Skill3 = (Skill)Activator.CreateInstance(type);
        type = Type.GetType(CSVUtils.GetFileName(PassifData.Name));
        Passif = (Skill)Activator.CreateInstance(type);
    }
}
