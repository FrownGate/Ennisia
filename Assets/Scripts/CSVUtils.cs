using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class CSVUtils
{
    public static string[] SplitCSVLine(string line)
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

    public static string GetFileName(string name)
    {
        name = Regex.Replace(name, @"[^0-9a-zA-Z]+", ""); // Remove non-alphanumeric characters
        name = name.Replace(" ", ""); // Remove spaces
        return name;
    }
}