using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Ennisia/Skill")]
public class SkillSO : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public float DamageRatio;
    public float ShieldAmount;
    public float HealingAmount;
    public float BuffAmount;
    public int MaxCooldown;
    public float IgnoreDef;
    public int HitNumber;
    public bool AOE;
    public bool IsMagic;
    public bool IsPassive;

    public SkillType SkillType;
    //public Sprite Icon;
}

public enum SkillType
{
    NotSorted, Weapon, SupportPassif, SupportSkill, EnemySkill, EnemyPassif
}