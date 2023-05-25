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


    public void Summon()
    {
        foreach (var item in GetRandomHeroFromPool(GetGachaPool(PickRarity())))
        {
            Debug.Log(item.Value);

            // if (PlayFabManager.Instance.HasSupport(item.Key)) return;
            if (PlayFabManager.Instance.HasSupport(item.Key))
            {
                Debug.Log("You already have this support" + item.Value);
                return;
            }
            PlayFabManager.Instance.AddSupport(item.Key);
            Debug.Log("=====================================");
        }
    }



    public void Summon(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Summon();
        }
    }

    public void Summon(int amount, int guarantee)
    {
        for (int i = 0; i < amount; i++)
        {
            if (i == guarantee)
            {
                // guarantee
            }
            Summon();
        }
    }



    private Dictionary<int, string> GetGachaPool(string rarity)
    {
        string path = Application.dataPath + "/Editor/CSV/Supports.csv";
        string[] lines = System.IO.File.ReadAllLines(path);
        string[] headers = lines[0].Split(',');
        Dictionary<int, string> gachaPool = new();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = SplitCSVLine(lines[i]);
            if (values.Length != headers.Length)
            {
                Debug.LogError($"Error parsing line {i + 1} in CSV file. The number of values does not match the number of headers.");
                continue;
            }
            Dictionary<string, string> rowData = new();
            for (int j = 0; j < headers.Length; j++)
            {
                rowData[headers[j]] = values[j];
            }
            if (rowData["Rarity"] == rarity)
            {
                gachaPool.Add(int.Parse(rowData["ID"]), rowData["Name"]);
            }
        }
        return gachaPool.OrderBy(x => Guid.NewGuid()).ToDictionary(item => item.Key, item => item.Value);
    }


    private Dictionary<int, string> GetRandomHeroFromPool(Dictionary<int, string> pool)
    {
        System.Random random = new();
        int index = random.Next(pool.Count);
        // Debug.Log("=====================================");
        // Debug.Log(pool.ElementAt(index));
        return pool.Skip(index).Take(1).ToDictionary(item => item.Key, item => item.Value);
    }


    private string[] SplitCSVLine(string line)
    {
        List<string> values = new();
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


    private string PickRarity()
    {
        int range = 100;
        System.Random random = new();
        double roll = random.NextDouble() * range;

        string pickedRarity = "";
        if (roll <= _legendaryChance)
        {
            pickedRarity = "Legendary";
        }
        else if (roll <= _epicChance)
        {
            pickedRarity = "Epic";
        }
        else if (roll <= _rareChance)
        {
            pickedRarity = "Rare";
        }
        return pickedRarity;
    }
}