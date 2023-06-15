using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class EndlessTower : MonoBehaviour
{
    DynamicButtonGenerator generator;
    List<GameObject> buttons;
    List<MissionSO> ETSO;
    void Awake()
    {
        generator = GetComponent<DynamicButtonGenerator>();

        ETSO = Resources.LoadAll<MissionSO>($"SO/Missions/EndlessTower/").ToList();
        ETSO = ETSO.OrderBy(obj => obj.NumInChapter).ToList();

        buttons = generator.GenerateButtonsInSlider(ETSO.Count);

        int buttonIndex = 0;
        foreach (GameObject go in buttons)
        {
           go.name = ETSO[buttonIndex].name;
            TextMeshProUGUI buttonText = go.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = ETSO[buttonIndex].Name;

            EndlessTowerFloor floor = go.AddComponent<EndlessTowerFloor>();
            floor.floorSO = ETSO[buttonIndex];











            buttonIndex++;
        }

    }

}
