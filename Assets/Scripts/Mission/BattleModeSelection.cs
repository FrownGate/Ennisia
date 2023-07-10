using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static MissionManager;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class BattleModeSelection : MonoBehaviour
{
    private DynamicButtonGenerator _buttonGenerator;

    private void Start()
    {
        _buttonGenerator = GetComponent<DynamicButtonGenerator>();
        // Generate buttons based on the MissionType enum
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        var missionTypes = (MissionType[])System.Enum.GetValues(typeof(MissionType));

        var buttonCount = CountValidMissionTypes(missionTypes);
#if UNITY_STANDALONE
        var buttons = _buttonGenerator.AddButtonsToGridLayout(buttonCount);
#endif
#if UNITY_IOS || UNITY_ANDROID
        List<GameObject> buttons = buttonGenerator.GenerateButtonsInSlider(buttonCount);
#endif
        var buttonIndex = 0;
        for (var i = 0; i < missionTypes.Length; i++)
        {
            var missionType = missionTypes[i];

            // Ignore mission types that contain "Story"
            if (missionType.ToString().Contains("Story"))
                continue;

            // Set the button's name based on the MissionType enum value
            buttons[buttonIndex].name = missionType.ToString();
            var buttonText = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = missionType == MissionType.EndlessTower
                ? $"<b>Endless Tower</b>\n"
                : $"<b>{buttons[buttonIndex].name}</b>\n";
#if UNITY_IOS || UNITY_ANDROID
            buttonText.fontSize = 150;
#endif
            var sceneButton = buttons[buttonIndex].GetComponentInChildren<SceneButton>();

            sceneButton.Scene = missionType switch
            {
                MissionType.Raid or MissionType.Dungeon => "Raids&Dungeons", //TODO -> don't use strings
                _ => missionType.ToString(),
            };
            sceneButton.SetParam(missionType.ToString());
            buttonIndex++;
        }
    }

    private int CountValidMissionTypes(MissionType[] missionTypes)
    {
        return missionTypes.Count(t => !t.ToString().Contains("Story"));
    }
}