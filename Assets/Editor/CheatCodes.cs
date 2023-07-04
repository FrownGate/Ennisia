using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;
using CheatCode;

public class CheatCodeWindow : EditorWindow
{
    private static BattleSystem battleSystem;
    [MenuItem("Tools/Cheat Codes")]
    public static void OpenWindow()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "You must be playing in the - Battle - scene to use the Cheat Code Tool", "OK");
            return;
        }
        else if (SceneManager.GetActiveScene().name != "Battle")
        {
            EditorUtility.DisplayDialog("Error", "You must be in the - Battle - scene to use the Cheat Code Tool", "OK");
            return;
        }
        CheatCodeWindow window = GetWindow<CheatCodeWindow>();
        GUIContent icon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
        window.titleContent = new GUIContent("Cheat Code Tool", icon.image, "Cheat Code Tool");

        Debug.LogWarning("Open : " + battleSystem);

    }

    private void OnEnable()
    {
        // FIXME: CAREFUL, OnEnable is called BEFORE the OpenWindow Method
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CheatCodes.uxml");
        TemplateContainer labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();



        TextField codeInput = root.Q<TextField>("codeInput");
        Button activateButton = root.Q<Button>("activateButton");


        CheatCodeManager cheatCodeManager = new(battleSystem);
        Debug.LogWarning("On Enable : " + battleSystem);
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