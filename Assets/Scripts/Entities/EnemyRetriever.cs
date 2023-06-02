using System;
using System.Collections.Generic;
using System.IO;

public class EnemyLoader
{

    public Enemy LoadEnemyByName(string filePath, string enemyName)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                string name = values[1];

                if (name == enemyName)
                {
                    int id = int.Parse(values[0]);
                    string[] stats = new string[9];
                    int[] statNumbers = new int[9];

                    for (int i = 2; i < 11; i++)
                    {
                        stats[i - 2] = values[i];
                        statNumbers[i - 2] = int.Parse(values[i]);
                    }

                    string description = values[11];

                    return new Enemy(id, name, stats, statNumbers, description);
                }
            }
        }

        return null; // Enemy with the specified name was not found
    }



    public List<Enemy> LoadEnemies(string filePath)
    {
        List<Enemy> enemies = new List<Enemy>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                int id = int.Parse(values[0]);
                string name = values[1];
                string[] stats = new string[9];
                int[] statNumbers = new int[9];

                for (int i = 2; i < 11; i++)
                {
                    stats[i - 2] = values[i];
                    statNumbers[i - 2] = int.Parse(values[i]);
                }

                //for (int i = 2; i < 8; i++)
                //{
                //    stats[i - 2] = values[i];
                //}

                //for (int i = 8; i < 14; i++)
                //{
                //    statNumbers[i - 8] = int.Parse(values[i]);
                //}

                string description = values[11];

                Enemy enemy = new Enemy(id, name, stats, statNumbers, description);
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