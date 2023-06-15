using System.Collections;
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

    public static Dictionary<int, Difficulty> idToDifficulty = new()
    {
        { 1, Difficulty.Peaceful },
        { 2, Difficulty.Easy },
        { 3, Difficulty.Normal },
        { 4, Difficulty.Hard },
        { 5, Difficulty.Insane },
        { 6, Difficulty.Ultimate }
    };


    public List<GameObject> _raids;
    DynamicButtonGenerator generator;
    public List<MissionSO> RaidDiffs;
    public List<GameObject> _buttons;
    public string RaidName;


    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponentInParent<DynamicButtonGenerator>();
        RaidDiffs = RaidDiffs.OrderBy(obj => obj.NumInChapter).ToList();
    }


    private void OnMouseDown()
    {
        generator.buttonPrefab = Resources.Load<GameObject>("Prefabs/UI/RaidDifficulty");

        _buttons = generator.GenerateButtonsInSlider(RaidDiffs.Count);
        int buttonIndex = 0;
        foreach (GameObject go in _buttons)
        {
            Image image = go.GetComponentInChildren<Image>();
            go.name = RaidDiffs[buttonIndex].name;
            image.sprite = Resources.Load<Sprite>($"Textures/UI/Raids/Difficulty{RaidName}");

            TextMeshProUGUI buttonText = go.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = GetDifficultyById(RaidDiffs[buttonIndex].NumInChapter).ToString();

            RaidsDifficulty diff = image.gameObject.AddComponent<RaidsDifficulty>();
            diff.raidDiff = RaidDiffs[buttonIndex];

            buttonIndex++;

        }


        SceneButton[] scripts = FindObjectsOfType<SceneButton>();

        foreach (SceneButton script in scripts)
        {
            if (script.Scene == "MainMenu")
            {
                Debug.Log("Found object with MyScript and myVariable set to : " + script.gameObject.name);
                // Do something with the found object
                script.Scene = "Raids";
            }
        }


        foreach (GameObject gameObject in _raids)
        {
            Destroy(gameObject);
        }
    }

    Difficulty GetDifficultyById(int id)
    {
        if (idToDifficulty.ContainsKey(id))
            return idToDifficulty[id];

        Debug.LogWarning("Difficulty ID not found: " + id);
        return Difficulty.Peaceful; // Return a default difficulty or handle the error accordingly
    }

}