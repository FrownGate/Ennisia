using UnityEngine;

[CreateAssetMenu(fileName = "New Pet", menuName = "Ennisia/Pet")]
public class PetSO : ScriptableObject
{
    public int Id = 1; // utile ?
    public string Name;
    public string Rarity;
    public string Lore;
    public Sprite Icon;
    public int Level;
   
}