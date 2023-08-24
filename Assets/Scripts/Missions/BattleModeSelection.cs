using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            if (missionType.ToString().Contains("Story")) continue;

            // Set the button's name based on the MissionType enum value
            buttons[buttonIndex].name = missionType.ToString();
            var buttonText = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = missionType.ToString();

            if (!IsUnlocked(missionType))
            {
                var background = buttons[buttonIndex].GetComponent<Image>();
                background.color = new Color(1, 1, 1, 0.5f);
                buttonText.color = new Color(0, 0, 0, 0.5f);
            }

#if UNITY_IOS || UNITY_ANDROID
            buttonText.fontSize = 150;
#endif
            var sceneButton = buttons[buttonIndex].GetComponentInChildren<SceneButton>();

            sceneButton.Scene = missionType switch
            {
                MissionType.Raid or MissionType.Dungeon => "Raids&Dungeons", //TODO -> use serialized scene
                _ => missionType.ToString(),
            };
            sceneButton.SetParam(missionType.ToString()); //TODO -> use manager instead of params
            buttonIndex++;
        }
    }

    private bool IsUnlocked(MissionType missionType)
    {
        MissionSO firstMission = Resources.Load<MissionSO>($"SO/Missions/{missionType}");
        if (firstMission == null || firstMission.State == MissionState.Locked) return false;
        return true;
    }

    private int CountValidMissionTypes(MissionType[] missionTypes)
    {
        return missionTypes.Count(t => !t.ToString().Contains("Story"));
    }
}