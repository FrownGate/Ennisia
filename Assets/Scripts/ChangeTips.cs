using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;

public class ChangeTips : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI _tip;
    private static int _currentLine;
    private static int _lines;
    void Start()
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
            _currentLine = i + 1;
            string[] values = CSVUtils.SplitCSVLine(lines[i]);
            rowData[int.Parse(values[0])] = values[1];
            }
        int random = UnityEngine.Random.Range(1, lines.Length);
        _tip.text = rowData[random];
    }
}