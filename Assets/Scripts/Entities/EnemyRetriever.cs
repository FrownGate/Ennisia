using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

public class EnemyLoader
{
    private readonly Dictionary<Attribute, float> _stats = new();
    //TODO -> Use CSVUtils to get headers and lines

    public Enemy LoadEnemyByName(string filePath, string enemyName)
    {
        _stats.Clear();

        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');
            Dictionary<string, string> rowData = new();

            for (int i = 0; i < 9; i++)
            {
                _stats.Add((Attribute)i, 0);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                string name = values[1];

                if (name == enemyName)
                {
                    int id = int.Parse(values[0]);

                    // Skip the first two columns Id and Name
                    for (int i = 2; i < 11; i++)
                    {
                        if (Enum.TryParse(rowData.ToArray()[i].Key, out Attribute stat))
                        {
                            _stats[stat] = int.Parse(rowData.ToArray()[i].Value);
                        }
                        else
                        {
                            Debug.LogError($" EnemyRetriever:LoadEnemies... Stat {rowData.ToArray()[i].Key} not found");
                        }
                    }

                    string description = values[11];

                    return new Enemy(id, name, _stats, description);
                }
            }
        }

        return null; // Enemy with the specified name was not found
    }

    public Enemy LoadEnemyById(string filePath, int enemyId)
    {
        _stats.Clear();

        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');
            Dictionary<string, string> rowData = new();

            for (int i = 0; i < 9; i++)
            {
                _stats.Add((Attribute)i, 0);

            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                int id = int.Parse(values[0]);

                if (id == enemyId)
                {
                    string name = values[1];

                    // Skip the first two columns Id and Name
                    for (int i = 2; i < 11; i++)
                    {
                        if (Enum.TryParse<Attribute>(rowData.ToArray()[i].Key, out Attribute stat))
                        {
                            _stats[stat] = int.Parse(rowData.ToArray()[i].Value);
                        }
                        else
                        {
                            Debug.LogError($" EnemyRetriever:LoadEnemies... Stat {rowData.ToArray()[i].Key} not found");
                        }
                    }
                    string description = values[11];

                    return new Enemy(id, name, _stats, description);
                }
            }
        }

        return null; // Enemy with the specified name was not found
    }

    // TODO: To Enemy 

    public List<Entity> LoadEnemies(string filePath)
    {
        _stats.Clear();
        // TODO: To Enemy 
        List<Entity> enemies = new();

        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');

            Dictionary<string, string> rowData = new();
            for (int i = 0; i < 9; i++)
            {
                // Skip the first two columns Id and Name
                _stats.Add((Attribute)i, 0);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                for (var i = 0; i < headers.Length; i++) rowData[headers[i]] = values[i];

                int id = int.Parse(values[0]);
                string name = values[1];

                // Skip the first two columns Id and Name
                for (int i = 2; i < 11; i++)
                {
                    if (Enum.TryParse<Attribute>(rowData.ToArray()[i].Key, out Attribute stat))
                    {
                        _stats[stat] = int.Parse(rowData.ToArray()[i].Value);
                    }
                    else
                    {
                        Debug.LogError($" EnemyRetriever:LoadEnemies... Stat {rowData.ToArray()[i].Key} not found");
                    }
                }

                string description = values[11];

                Enemy enemy = new(id, name, _stats, description);
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    // private List<string[]> LoadCSV(string filePath)
    // {
    //     List<string[]> data = new List<string[]>();

    //     using (StreamReader reader = new StreamReader(filePath))
    //     {
    //         while (!reader.EndOfStream)
    //         {
    //             string line = reader.ReadLine();
    //             string[] values = line.Split(',');

    //             data.Add(values);
    //         }
    //     }

    //     return data;
    // }
}