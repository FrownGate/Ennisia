
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class Raids : MonoBehaviour
{
    DynamicButtonGenerator generator;
    List<GameObject> buttons;
    MissionSO[] raidsSO;
    // Start is called before the first frame update
    void Awake()
    {
        generator = GetComponent<DynamicButtonGenerator>();

        raidsSO = Resources.LoadAll<MissionSO>($"SO/Missions/Raid/");

        List<string> uniqueNames = GetUniqueNames();
        foreach (string name in uniqueNames)
        {
            Debug.Log(name);
        }


        buttons = generator.GenerateButtonsInSlider(uniqueNames.Count);
        int buttonIndex = 0;
        foreach (GameObject go in buttons)
        {
            Image image = go.GetComponentInChildren<Image>();
            go.name = uniqueNames[buttonIndex];
            image.sprite = Resources.Load<Sprite>($"Textures/UI/Raids/{uniqueNames[buttonIndex]}");
            buttonIndex++;

            RaidsButton diff = image.gameObject.AddComponent<RaidsButton>();
            diff.RaidName = go.name;
            diff._raids = buttons;
            diff.RaidDiffs = GetScriptableObjectsByName(go.name);

        }

    }

    public List<string> GetUniqueNames()
    {
        HashSet<string> uniqueNames = new();

        foreach (MissionSO obj in raidsSO)
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

        foreach (MissionSO obj in raidsSO)
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

