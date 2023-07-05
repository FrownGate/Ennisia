using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using static MissionManager;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class BattleModeSelection : MonoBehaviour
{
    private DynamicButtonGenerator buttonGenerator;

    private void Start()
    {
        buttonGenerator = GetComponent<DynamicButtonGenerator>();
        // Generate buttons based on the MissionType enum
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        MissionType[] missionTypes = (MissionType[])System.Enum.GetValues(typeof(MissionType));

        int buttonCount = CountValidMissionTypes(missionTypes);
        List<GameObject> buttons = buttonGenerator.AddButtonsToGridLayout(buttonCount);

        int buttonIndex = 0;
        for (int i = 0; i < missionTypes.Length; i++)
        {
            MissionType missionType = missionTypes[i]; 

            // Ignore mission types that contain "Story"
            if (missionType.ToString().Contains("Story"))
                continue;

            // Set the button's name based on the MissionType enum value
            buttons[buttonIndex].name = missionType.ToString();
            TextMeshProUGUI buttonText = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"<b>{buttons[buttonIndex].name}</b>\n";
            SceneButton sceneButton = buttons[buttonIndex].GetComponentInChildren<SceneButton>();
            if(buttons[buttonIndex].name == "Expedition") 
                sceneButton.Scene = buttons[buttonIndex].name+"Selection";
            else sceneButton.Scene = buttons[buttonIndex].name;
            buttonIndex++;
        }
    }

    private int CountValidMissionTypes(MissionType[] missionTypes)
    {
        int count = 0;
        for (int i = 0; i < missionTypes.Length; i++)
        {
            if (!missionTypes[i].ToString().Contains("Story"))
                count++;
        }
        return count;
    }
}