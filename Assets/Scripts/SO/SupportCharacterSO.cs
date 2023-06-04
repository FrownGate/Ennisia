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
    public Skill PrimarySkill;
    public Skill SecondarySkill;
    public string Description;
    public string Catchphrase;
    public Sprite Icon;
}