using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class EndlessTower : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private List<GameObject> _buttons;
    private List<MissionSO> _etSO;

    void Awake()
    {
        _generator = GetComponent<DynamicButtonGenerator>();

        _etSO = Resources.LoadAll<MissionSO>($"SO/Missions/EndlessTower/").ToList();
        _etSO = _etSO.OrderBy(obj => obj.NumInChapter).ToList();

        _buttons = _generator.GenerateButtonsInSlider(_etSO.Count);

        int buttonIndex = 0;

        foreach (GameObject go in _buttons)
        {
            go.name = _etSO[buttonIndex].name;
            TextMeshProUGUI buttonText = go.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _etSO[buttonIndex].Name;

            EndlessTowerFloor floor = go.AddComponent<EndlessTowerFloor>();
            floor.FloorSO = _etSO[buttonIndex];
            buttonIndex++;
        }
    }
}