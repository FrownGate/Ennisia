using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;


using Currencies;

public class ToolCurrencies : EditorWindow
{

    [MenuItem("Currencies/Gestion")]
    public static void ShowWindow()
    {
        GetWindow<ToolCurrencies>("Currencies");
        ToolCurrencies window = GetWindow<ToolCurrencies>("Currencies");
        window.titleContent = new GUIContent("Currencies");
        window.minSize = new Vector2(500, 300);
        window.maxSize = new Vector2(900, 600);
    }

    public void CreateGUI()
    {
        rootVisualElement.Clear();
        VisualElement main = new VisualElement();
    }
}