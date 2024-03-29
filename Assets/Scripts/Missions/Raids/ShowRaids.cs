using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class ShowRaids : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private List<GameObject> _buttons;
    private MissionSO[] _raidsSO;

    void Start()
    {
        _generator = GetComponent<DynamicButtonGenerator>();

        var type = ScenesManager.Instance.Params;
        _raidsSO = Resources.LoadAll<MissionSO>($"SO/Missions/{type}/");

        List<string> uniqueNames = GetUniqueNames();
        foreach (string name in uniqueNames)
        {
            Debug.Log(name);
        }

        _buttons = _generator.GenerateButtonsInSlider(uniqueNames.Count);
        int buttonIndex = 0;
        foreach (GameObject go in _buttons)
        {
            Image image = go.GetComponentInChildren<Image>();
            go.name = uniqueNames[buttonIndex];
            image.sprite = Resources.Load<Sprite>($"Textures/UI/{type}/{uniqueNames[buttonIndex]}");
            buttonIndex++;

            ShowRaidDifficulties diff = image.gameObject.AddComponent<ShowRaidDifficulties>();
            diff.RaidName = go.name;
            diff.Raids = _buttons;
            diff.RaidDiffs = GetScriptableObjectsByName(go.name);
        }
    }

    public List<string> GetUniqueNames()
    {
        HashSet<string> uniqueNames = new();

        foreach (MissionSO obj in _raidsSO)
        {
            // Extract the name without numbers using regular expressions
            string nameWithoutNumbers = Regex.Replace(obj.name, @"[\d-.]", "");

            // Add the name to the uniqueNames set
            uniqueNames.Add(nameWithoutNumbers);
        }

        // Convert the set to a list and return
        return new List<string>(uniqueNames);
    }

    public List<MissionSO> GetScriptableObjectsByName(string name)
    {
        Dictionary<string, List<MissionSO>> scriptableObjectsByName = new();

        foreach (MissionSO obj in _raidsSO)
        {
            string nameWithoutNumbers = Regex.Replace(obj.name, @"[\d-.]", "");

            if (!scriptableObjectsByName.ContainsKey(nameWithoutNumbers))
            {
                scriptableObjectsByName[nameWithoutNumbers] = new List<MissionSO>();
            }

            scriptableObjectsByName[nameWithoutNumbers].Add(obj);
        }

        return scriptableObjectsByName[name];
    }
}