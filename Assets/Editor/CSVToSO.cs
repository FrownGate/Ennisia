using UnityEngine;
using UnityEditor;
using System.IO;
using JetBrains.Annotations;

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

            AssetDatabase.CreateAsset(equipment, $"Assets/Equipments/DebugGears/{equipment.Type}.asset");

        }

    AssetDatabase.SaveAssets();

    }

}
