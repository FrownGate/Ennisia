using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Ennisia/Quest")]
public class QuestSO : ScriptableObject
{
    public string questName;
    public string questDescription;
    public int crystlasAmount;
    public int goldAmount;
    public int enrgyAmount;



}