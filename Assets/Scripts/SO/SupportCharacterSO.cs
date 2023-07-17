using UnityEngine;

[CreateAssetMenu(fileName = "New Support", menuName = "Ennisia/Support")]
public class SupportCharacterSO : SkillsToInitSO
{
    public int Id = 1;
    public string Name;
    public Rarity Rarity;
    public string Race;
    public string Job;
    public ElementType Element;
    public string Description;
    public string Catchphrase;
    //public Sprite Icon;
    [HideInInspector] public SupportHUD Button;
}