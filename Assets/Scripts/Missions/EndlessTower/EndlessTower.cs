using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class EndlessTower : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private ScrollRect _scrollRect;
    private List<GameObject> _buttons;
    private List<MissionSO> _etSO;
    private int _nextFloor;

    void Awake()
    {
        _generator = GetComponent<DynamicButtonGenerator>();
        _scrollRect = GetComponent<ScrollRect>();

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
            
            if (_etSO[buttonIndex].State == MissionState.Unlocked)
            {
                // set the position of the slider to the first unlocked floor
                _scrollRect.verticalNormalizedPosition = 1 - (float)buttonIndex / (_etSO.Count - 1);
            }

            buttonIndex++;

        }

    }
}