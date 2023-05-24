using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill/New")]
public class SkillData : ScriptableObject
{
    /*Weapon weapon;*/
    public int hitNb;
    public float healingAmount;
    public float damageAmount;
    public float shieldAmount;
    public float penDef;
    public string description;
    public string skillName;
    public bool isAfter;
    public bool AOE;
    public bool use;
    public bool isMagic;
    Texture2D texture;


}
