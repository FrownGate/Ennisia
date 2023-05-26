using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill/New Data")]
public class SkillData : ScriptableObject
{
    /*Weapon weapon;*/
    public int id;
    public string skillName;
    public string description;
    public float damageAmount;
    public float shieldAmount;
    public float healingAmount;
    public int maxCooldown;
    public float penDef;
    public int hitNb;
    public bool isAfter;
    public bool AOE;
    public bool isMagic;
    Texture2D texture;


}
