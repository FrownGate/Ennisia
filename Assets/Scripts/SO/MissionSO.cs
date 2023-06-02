using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Mission Scriptable Object Class
[CreateAssetMenu(fileName = "New Mission", menuName = "Game/Mission")]
public class MissionSO : ScriptableObject
{
    public int ID;
    public string Name;
    public MissionType MissionType;
    public int EnergyCost;
    public bool Unlocked;
    public List<WavesSO> Waves;
    public int DialogueID;
    // Add additional mission data as needed
}
