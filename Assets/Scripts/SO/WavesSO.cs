using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Mission Scriptable Object Class
[CreateAssetMenu(fileName = "New Wave", menuName = "Game/Waves")]
public class WavesSO : ScriptableObject
{
    public int ID;
    public string Name;
    public List<Enemy> Enemies;
    // Add additional mission data as needed
}
