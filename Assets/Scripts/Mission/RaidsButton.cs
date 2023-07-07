using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    Peaceful,
    Easy,
    Normal,
    Hard,
    Insane,
    Ultimate
}

public class RaidsButton : MonoBehaviour
{
    public static Dictionary<int, Difficulty> IdToDifficulty = new()
    {
        { 1, Difficulty.Peaceful },
        { 2, Difficulty.Easy },
        { 3, Difficulty.Normal },
        { 4, Difficulty.Hard },
        { 5, Difficulty.Insane },
        { 6, Difficulty.Ultimate }
    }; //TODO -> remove and use (int)Difficulty.Peaceful for example

    public List<GameObject> Raids;
    public List<MissionSO> RaidDiffs;
    public List<GameObject> Buttons;
    public string RaidName;

    private DynamicButtonGenerator _generator;

    // Start is called before the first frame update
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
            buttonText.text = GetDifficultyById(RaidDiffs[buttonIndex].NumInChapter).ToString();

            RaidsDifficulty diff = image.gameObject.AddComponent<RaidsDifficulty>();
            diff.RaidDiff = RaidDiffs[buttonIndex];

            buttonIndex++;
        }

        SceneButton[] scripts = FindObjectsOfType<SceneButton>();

        foreach (SceneButton script in scripts)
        {
            if (script.Scene != "MainMenu") continue;
            Debug.Log("Found object with MyScript and myVariable set to : " + script.gameObject.name);
            // Do something with the found object
            script.Scene = "Raids";
        }

        foreach (GameObject gameObject in Raids)
        {
            Destroy(gameObject);
        }
    }

    Difficulty GetDifficultyById(int id)
    {
        if (IdToDifficulty.ContainsKey(id)) return IdToDifficulty[id];

        Debug.LogWarning("Difficulty ID not found: " + id);
        return Difficulty.Peaceful; // Return a default difficulty or handle the error accordingly
    }
}