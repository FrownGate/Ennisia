using System;
using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class ShowQuests : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private List<GameObject> _buttons;
    private QuestSO[] _quests;
    // Start is called before the first frame update
    private void Start()
    {
        throw new NotImplementedException();
    }
}