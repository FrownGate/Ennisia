using System.Collections.Generic;
using System.Linq;
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
#if UNITY_STANDALONE
        List<GameObject> buttons = buttonGenerator.AddButtonsToGridLayout(buttonCount);
#endif
#if UNITY_IOS || UNITY_ANDROID
        List<GameObject> buttons = buttonGenerator.GenerateButtonsInSlider(buttonCount);
#endif
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
            buttonText.text = missionType == MissionType.EndlessTower
                ? $"<b>Endless Tower</b>\n"
                : $"<b>{buttons[buttonIndex].name}</b>\n";
#if UNITY_IOS || UNITY_ANDROID
            buttonText.fontSize = 150;
#endif
            SceneButton sceneButton = buttons[buttonIndex].GetComponentInChildren<SceneButton>();
            if (missionType == MissionType.Expedition)
                sceneButton.Scene = buttons[buttonIndex].name + "Selection";
            else sceneButton.Scene = buttons[buttonIndex].name;
            buttonIndex++;
        }
    }

    private int CountValidMissionTypes(MissionType[] missionTypes)
    {
        return missionTypes.Count(t => !t.ToString().Contains("Story"));
    }
}