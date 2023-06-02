using System;
using System.Collections.Generic;
using System.IO;

public class EnemyLoader
{

    public Enemy LoadEnemyByName(string filePath, string enemyName)
    {
        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');

            Dictionary<string, int> stats = new();

            for (int i = 2; i < 11; i++)
            {
                // Skip the first two columns Id and Name
                stats.Add(headers[i], 0);
            }


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                string name = values[1];

                if (name == enemyName)
                {
                    int id = int.Parse(values[0]);


                    for (int i = 2; i < 11; i++)
                    {
                        stats[headers[i]] = int.Parse(values[i]);
                    }

                    string description = values[11];

                    return new Enemy(id, name, stats, description);
                }
            }
        }

        return null; // Enemy with the specified name was not found
    }

    public Enemy LoadEnemyById(string filePath, int enemyId)
    {
        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');

            Dictionary<string, int> stats = new();

            for (int i = 2; i < 11; i++)
            {
                // Skip the first two columns Id and Name
                stats.Add(headers[i], 0);
            }


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                int id = int.Parse(values[0]);

                if (id == enemyId)
                {
                    string name = values[1];


                    for (int i = 2; i < 11; i++)
                    {
                        stats[headers[i]] = int.Parse(values[i]);
                    }

                    string description = values[11];

                    return new Enemy(id, name, stats, description);
                }
            }
        }

        return null; // Enemy with the specified name was not found
    }



    public List<Enemy> LoadEnemies(string filePath)
    {
        List<Enemy> enemies = new();

        using (StreamReader reader = new(filePath))
        {
            string headerLine = reader.ReadLine(); // Skip the header line
            string[] headers = headerLine.Split(',');
            Dictionary<string, int> stats = new();

            for (int i = 2; i < 11; i++)
            {
                // Skip the first two columns Id and Name
                stats.Add(headers[i], 0);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                int id = int.Parse(values[0]);
                string name = values[1];



                // Loop through the statNumbers arrays    
                for (int i = 2; i < 11; i++)
                {
                    stats[headers[i]] = int.Parse(values[i]);
                }



                string description = values[11];

                Enemy enemy = new(id, name, stats, description);
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