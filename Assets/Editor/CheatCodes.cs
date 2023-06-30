using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using CheatCode;

public class CheatCodeWindow : EditorWindow
{
    [MenuItem("Tools/Cheat Codes")]
    public static void OpenWindow()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "You must be in play mode to use this tool", "Ok");
            return;
        }
        CheatCodeWindow window = GetWindow<CheatCodeWindow>();
        GUIContent icon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
        window.titleContent = new GUIContent("Cheat Code Tool", icon.image, "Cheat Code Tool");
    }

    private void OnEnable()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CheatCodes.uxml");
        TemplateContainer labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);


        TextField codeInput = root.Q<TextField>("codeInput");
        Button activateButton = root.Q<Button>("activateButton");


        CheatCodeManager cheatCodeManager = new();
        activateButton.clickable.clicked += () =>
        {
            cheatCodeManager.CheckAndActivateCheat(codeInput.value);
            ClearInput(codeInput);

        };
    }
    private void ClearInput(TextField textField)
    {
        textField.value = "";
    }
}