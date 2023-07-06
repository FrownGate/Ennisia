using UnityEngine;

[CreateAssetMenu(fileName = "New Summon Banner", menuName = "Ennisia/Summon Banner")]
public class SummonBannerSO : ScriptableObject
{
    //TODO -> Add Banner Image
    public string Name;
    public SupportCharacterSO MainCharacter;
    public SupportCharacterSO MinorCharacter;
}