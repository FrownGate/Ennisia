using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class GachaSystem : MonoBehaviour
{
    private double _legendaryChance = 0.5;
    private double _epicChance = 9.5;
    private double _rareChance = 90;


    public void GachaRoll()
    {
        int range = 100;
        System.Random random = new();
        double roll = random.NextDouble() * range;
        if (roll <= _legendaryChance)
        {
            Debug.Log(GetGachaPool("Legendary"));
        }
        else if (roll <= _epicChance)
        {
            Debug.Log(GetGachaPool("Epic"));
        }
        else if (roll <= _rareChance)
        {
            Debug.Log(GetGachaPool("Rare"));
        }

    }

    public void GachaRoll(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GachaRoll();
        }
    }

    public void GachaRoll(int amount, int guarantee)
    {
        for (int i = 0; i < amount; i++)
        {
            if (i == guarantee)
            {
                // guarantee
            }
            GachaRoll();
        }
    }

    private Dictionary<int, string> GetGachaPool(string rarity)
    {
        string path = Application.dataPath + "/Editor/CSV/Supports.csv";
        string[] lines = System.IO.File.ReadAllLines(path);
        string[] headers = lines[0].Split(',');
        Dictionary<int, string> gachaPool = new Dictionary<int, string>();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = CSVToSO.SplitCSVLine(lines[i]);
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
            if (rowData["Rarity"] == rarity)
            {
                gachaPool.Add(int.Parse(rowData["ID"]), rowData["Name"]);
            }
        }
        return gachaPool;
    }


}