using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CSVToSO : EditorWindow
{
    enum TypeCSV
    {
        supports,enemies,skills
    }
    [MenuItem("Tools/CSV to SO")]
    public static void ShowWindow()
    {
        GetWindow<CSVToSO>("CSV to SO");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Scriptable Objects from CSV", EditorStyles.boldLabel);
        GUILayout.Space(25);
        if (GUILayout.Button("Supports"))
        {
            string path = Application.dataPath + "/Editor/CSV/Supports.csv"; // EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                CreateScriptableObjectsFromCSV(TypeCSV.supports, path);
            }
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Enemies"))
        {
            string path = Application.dataPath + "/Editor/CSV/Enemies.csv"; // EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                CreateScriptableObjectsFromCSV(TypeCSV.enemies, path);
            }
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Skills"))
        {
            string path = Application.dataPath + "/Editor/CSV/Skills.csv"; // EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                CreateScriptableObjectsFromCSV(TypeCSV.skills, path);
            }
        }
    }

    private void CreateScriptableObjectsFromCSV(TypeCSV type, string filePath)
    {
        string[] lines = System.IO.File.ReadAllLines(filePath);

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file is empty or missing headers.");
            return;
        }

        string[] headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = SplitCSVLine(lines[i]);

            if (values.Length != headers.Length)
            {
                Debug.LogError($"Error parsing line {i + 1} in CSV file. The number of values does not match the number of headers.");
                continue;
            }

            Dictionary<string, string> rowData = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                rowData[headers[j]] = values[j];
            }
            switch (type)
            {
                case TypeCSV.supports:
                     createSupportSO(rowData);
                    break;
                case TypeCSV.enemies:
                    createEnemySO(rowData);
                    break;
                case TypeCSV.skills:
                    createSkillDataSO(rowData);
                    break;
                default:
                    break;
            }


        }
    }

    private static void createSupportSO(Dictionary<string, string> rowData)
    {
        SupportsCharactersSO scriptableObject = ScriptableObject.CreateInstance<SupportsCharactersSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.Rarity = rowData["Rarity"];
        scriptableObject.Race = rowData["Race"];
        scriptableObject.Job = rowData["Class"];
        scriptableObject.Description = rowData["Description"].Replace("\"", string.Empty);
        scriptableObject.Catchphrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        
        string savePath = $"Assets/Resources/SO/SupportsCharacter/{scriptableObject.Rarity}/{scriptableObject.Id}-{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static void createEnemySO(Dictionary<string, string> rowData)
    {
        //EnemiesSO scriptableObject = ScriptableObject.CreateInstance<EnemiesSO>();
        //scriptableObject.id = int.Parse(rowData["ID"]);
        //scriptableObject.suppportName = rowData["Name"];
        //scriptableObject.rarity = rowData["Rarity"];
        //scriptableObject.race = rowData["Race"];
        //scriptableObject.supportClass = rowData["Class"];
        //scriptableObject.passif = int.Parse(rowData["Passif"]);
        //scriptableObject.skill = int.Parse(rowData["Skill"]);
        //scriptableObject.description = rowData["Description"].Replace("\"", string.Empty);
        //scriptableObject.catchPhrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        //// Save the scriptable object
        //string savePath = $"Assets/Resources/SO/Enemies/{scriptableObject.id}-{scriptableObject.suppportName}.asset";
        //AssetDatabase.CreateAsset(scriptableObject, savePath);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }  
    private static void createSkillDataSO(Dictionary<string, string> rowData)
    {
        SkillData scriptableObject = ScriptableObject.CreateInstance<SkillData>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["skillName"];
        scriptableObject.Description = rowData["description"].Replace("\"", string.Empty);
        scriptableObject.DamageAmount = float.Parse( rowData["damageAmount"]);
        scriptableObject.ShieldAmount = float.Parse( rowData["shieldAmount"]);
        scriptableObject.HealingAmount = float.Parse( rowData["healingAmount"]);
        scriptableObject.IgnoreDef = float.Parse( rowData["penDef"]);
        scriptableObject.HitNumber = int.Parse( rowData["hitNb"]);
        scriptableObject.MaxCooldown = int.Parse( rowData["maxCooldown"]);
        scriptableObject.IsAfter = bool.Parse( rowData["isAfter"]);
        scriptableObject.AOE = bool.Parse( rowData["AOE"]);
        scriptableObject.IsMagic = bool.Parse( rowData["isMagic"]);

        // Save the scriptable object
        string savePath = $"Assets/Resources/SO/Skills/{scriptableObject.Name.Replace(" ", string.Empty).Replace("\u2019", string.Empty).Replace("!", string.Empty)}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private string[] SplitCSVLine(string line)
    {
        List<string> values = new List<string>();
        bool insideQuotes = false;
        string currentValue = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '\"')
            {
                insideQuotes = !insideQuotes;
            }
            else if (c == ',' && !insideQuotes)
            {
                values.Add(currentValue);
                currentValue = "";
            }
            else
            {
                currentValue += c;
            }
        }

        values.Add(currentValue);

        return values.ToArray();
    }
}
