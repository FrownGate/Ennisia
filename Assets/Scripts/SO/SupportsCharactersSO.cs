using UnityEngine;

[CreateAssetMenu(fileName = "New Support", menuName = "Support/New")]
public class SupportsCharactersSO : ScriptableObject
{
    public int Id = 1;
    public string Name;
    public string Rarity;
    public string Race;
    public string Job;
    public Skill PrimarySkill;
    public Skill SecondarySkill;
    public string Description;
    public string Catchphrase;
    public Sprite Icon;
}