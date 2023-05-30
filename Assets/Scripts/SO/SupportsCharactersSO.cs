using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Support", menuName = "Support/New")]
public class SupportsCharactersSO : ScriptableObject
{
    public int id = 1;
    public string suppportName;
    public string rarity;
    public string race;
    public string supportClass;
    public Skill passif;
    public Skill skill;
    public string description;
    public string catchPhrase;
}
