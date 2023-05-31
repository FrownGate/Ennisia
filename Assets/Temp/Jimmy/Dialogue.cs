using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Rendering.DebugUI;

public class Dialogue 
{
    public List<string> name;
    public List<string> dialogues;

    public Dialogue()
    {
        name = new();
        dialogues = new();
    }

    public void readCSVFile(int id)
    {
        Debug.Log(Application.dataPath);

        string path = Application.dataPath + $"/Resources/CSV/DialogueSystem_{id}.csv";
        Debug.Log(path);


        string[] lines = System.IO.File.ReadAllLines(path);

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

            name.Add(values[0]);
            dialogues.Add(values[1]);

        }
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
