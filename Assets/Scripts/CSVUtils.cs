using System.Collections.Generic;
using System.ComponentModel;

public static class CSVUtils
{
    public static string[] SplitCSVLine(string line)
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

    public static string GetFileName(string name)
    {
        return name.Replace(" ", string.Empty).Replace("\u2019", string.Empty).Replace("!", string.Empty).Replace("'", string.Empty);
    }
}