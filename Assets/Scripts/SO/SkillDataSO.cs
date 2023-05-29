using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill/New Data")]
public class SkillData : ScriptableObject
{
    /*Weapon weapon;*/
    public int Id;
    public string Name;
    public string Description;
    public float DamageAmount;
    public float ShieldAmount;
    public float HealingAmount;
    public int MaxCooldown;
    public float IgnoreDef;
    public int HitNumber;
    public bool IsAfter;
    public bool AOE;
    public bool IsMagic;
    public Sprite Icon;
}