using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public List<string> name;
    public List<string> dialogues;
    public Dictionary<string, Sprite> sprites;

    public Dialogue(int id)
    {
        name = new();
        dialogues = new();
        sprites = new();

        ReadCSVFile(id);
        // LoadSprites(id);
    }

    public void ReadCSVFile(int id)
    {
        string path = Application.dataPath + $"/Resources/CSV/Dialogue/DialogueSystem_{id}.csv";
        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file is empty or missing headers.");
            return;
        }

        string[] headers = lines[0].Split(';');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length != headers.Length)
            {
                Debug.LogError($"Error parsing line {i + 1} in CSV file. The number of values does not match the number of headers.");
                continue;
            }

            name.Add(values[0]);
            dialogues.Add(values[1]);
        }
    }
}