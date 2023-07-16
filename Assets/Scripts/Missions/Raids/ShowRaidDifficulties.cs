using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowRaidDifficulties : MonoBehaviour
{
    public List<GameObject> Raids;
    public List<MissionSO> RaidDiffs;
    public List<GameObject> Buttons;
    public string RaidName;

    private DynamicButtonGenerator _generator;

    void Start()
    {
        _generator = GetComponentInParent<DynamicButtonGenerator>();
        RaidDiffs = RaidDiffs.OrderBy(obj => obj.NumInChapter).ToList();
    }

    private void OnMouseDown()
    {
        _generator.ButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/RaidDifficulty");

        Buttons = _generator.GenerateButtonsInSlider(RaidDiffs.Count);
        int buttonIndex = 0;

        foreach (GameObject go in Buttons)
        {
            Image image = go.GetComponentInChildren<Image>();
            go.name = RaidDiffs[buttonIndex].name;
            image.sprite = Resources.Load<Sprite>($"Textures/UI/Raid/Difficulty{RaidName}");

            TextMeshProUGUI buttonText = go.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = ((Difficulty)RaidDiffs[buttonIndex].NumInChapter).ToString();

            SetRaidDifficulty diff = image.gameObject.AddComponent<SetRaidDifficulty>();
            diff.RaidDiff = RaidDiffs[buttonIndex];

            buttonIndex++;
        }

        SceneButton[] scripts = FindObjectsOfType<SceneButton>(); //TODO -> use serialized field

        foreach (SceneButton script in scripts)
        {
            if (script.Scene != "MainMenu") continue;
            Debug.Log("Found object with MyScript and myVariable set to : " + script.gameObject.name);
            // Do something with the found object
            script.Scene = "Raids"; //TODO -> use serialized scene
        }

        foreach (GameObject gameObject in Raids) Destroy(gameObject);
    }
}