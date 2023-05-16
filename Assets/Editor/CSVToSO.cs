using UnityEngine;
using UnityEditor;
using System.IO;


public class CSVToSO
{
    private static string EquipmentCSVPath = "/Editor/CSV/EquipmentStats.csv";
    [MenuItem("Utilities/Create equipment SO from CSV")]
    public static void GenerateEquipment()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + EquipmentCSVPath);
        foreach (string line in allLines)
        {
            string[] splitData = line.Split(",");

            EquipmentSO equipment = ScriptableObject.CreateInstance<EquipmentSO>();

            equipment.Name = splitData[0];
            equipment.Type = splitData[1];
            equipment.CommonMin = int.Parse(splitData[2]);
            equipment.CommonMax = int.Parse(splitData[3]);
            equipment.RareMin = int.Parse(splitData[4]);
            equipment.RareMax = int.Parse(splitData[5]);
            equipment.EpicMin = int.Parse(splitData[6]);
            equipment.EpicMax = int.Parse(splitData[7]);
            equipment.LegendaryMin = int.Parse(splitData[8]);
            equipment.LegendaryMax = int.Parse(splitData[9]);
            AssetDatabase.CreateAsset(equipment, $"Assets/Equipments/DebugGears/{equipment.Name}.asset");

        }

        AssetDatabase.SaveAssets();

    }


}
