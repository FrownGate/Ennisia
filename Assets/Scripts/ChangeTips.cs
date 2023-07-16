using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class ChangeTips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tip;
    private static int _currentLine;
    private static int _lines;

    private void Start()
    {
        string filePath = Application.dataPath + $"/Resources/CSV/Tips.csv";
        string[] lines = File.ReadAllLines(filePath);
        _lines = lines.Length;

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file is empty or missing headers.");
            return;
        }

        Dictionary<int, string> rowData = new();

        for (int i = 1; i < lines.Length; i++)
        {
            _currentLine = i;
            string[] values = CSVUtils.SplitCSVLine(lines[i]);
            rowData[_currentLine] = values[1];
        }

        int random = Random.Range(1, _currentLine + 1);
        _tip.text = rowData[random];
    }
}