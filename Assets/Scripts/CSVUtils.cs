using System.Collections.Generic;
using System.Text.RegularExpressions;

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
}