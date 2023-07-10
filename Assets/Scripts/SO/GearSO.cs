using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGear", menuName = "Ennisia/Gear")]
public class GearSO : SkillsToInitSO
{
    public int Id;
    public string Name;
    public int Level = 1;
    public GearType Type;
    public Rarity Rarity;
    public Attribute Attribute;
    public float StatValue;
    public string Description;
    //public Sprite Icon;
    public SerializedDictionary<Attribute, float> Substats;

    //Weapons
    public WeaponType WeaponType;
    public bool IsMagic;

    public void Unequip()
    {
        Id = 0; //If Id = 0 then no gear is equipped
    }
}