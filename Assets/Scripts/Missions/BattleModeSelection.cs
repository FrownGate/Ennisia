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

            var sceneButton = buttons[buttonIndex].GetComponentInChildren<SceneButton>();

            if (!IsUnlocked(missionType))
            {
                var background = buttons[buttonIndex].GetComponent<Image>();
                background.color = Color.gray;
                buttonText.color = new Color(0, 0, 0, 0.5f);
                Destroy(sceneButton);
            }
            else
            {
                sceneButton.Scene = missionType switch
                {
                    MissionType.Raid or MissionType.Dungeon => "Raids&Dungeons", //TODO -> use serialized scene
                    _ => missionType.ToString(),
                };
                sceneButton.SetParam(missionType.ToString()); //TODO -> use manager instead of params
            }

#if UNITY_IOS || UNITY_ANDROID
            buttonText.fontSize = 150;
#endif
            buttonIndex++;
        }
    }

    private bool IsUnlocked(MissionType missionType)
    {
        MissionSO firstMission = PlayFabManager.Instance.Account.MissionsList[missionType][0];
        if (firstMission == null || firstMission.State == MissionState.Locked) return false;
        return true;
    }

    private int CountValidMissionTypes(MissionType[] missionTypes)
    {
        return missionTypes.Count(t => !t.ToString().Contains("Story"));
    }
}