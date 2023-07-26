using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CSVUtils
{
    public static string[] SplitCSVLine(string line)
    {
        List<string> values = new();
        bool insideQuotes = false;
        string currentValue = "";

        foreach (var c in line)
        {
            switch (c)
            {
                case '\"':
                    insideQuotes = !insideQuotes;
                    break;
                case ',' when !insideQuotes:
                    values.Add(currentValue);
                    currentValue = "";
                    break;
                default:
                    currentValue += c;
                    break;
            }
        }

        values.Add(currentValue);

        return values.ToArray();
    }

    public static string GetFileName(string name)
    {
        name = Regex.Replace(name, @"[^0-9a-zA-Z]+", ""); // Remove non-alphanumeric characters
        name = name.Replace(" ", ""); // Remove spaces
        return name;
    }

    public static string[] GetCSVLines(string csv)
    {
        string path = Application.dataPath + $"/Resources/CSV/{csv}.csv";
        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file is empty or missing headers.");
            return null;
        }

        return lines;
    }

    public static string[] GetCSVHeaders(string[] lines)
    {
        return lines[0].Split(',');
    }

    public static Dictionary<string, string> GetRowDatas(string[] lines, int index)
    {
        var rowData = new Dictionary<string, string>();

        string[] values = SplitCSVLine(lines[index]);
        string[] headers = GetCSVHeaders(lines);

        if (values.Length != headers.Length)
        {
            Debug.LogError($"Error parsing line {index + 1} in CSV file. The number of values does not match the number of headers.");
            return null;
        }

        for (var j = 0; j < headers.Length; j++) rowData[headers[j]] = values[j];

        return rowData;
    }
}