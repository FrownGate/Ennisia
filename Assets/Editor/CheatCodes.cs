using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CheatCodeWindow : EditorWindow
{
    private static BattleSystem _battleSystem;

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
    }

    private void OnEnable()
    {
        // FIXME: CAREFUL, OnEnable is called BEFORE the OpenWindow Method
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CheatCodes.uxml");
        TemplateContainer labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        _battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();

        TextField codeInput = root.Q<TextField>("codeInput");
        Button activateButton = root.Q<Button>("activateButton");
        Button removeButton = root.Q<Button>("removeButton");

        CheatCodeManager cheatCodeManager = new(_battleSystem);

        activateButton.clickable.clicked += () =>
        {
            cheatCodeManager.CheckAndActivateCheat(codeInput.value);
            ClearInput(codeInput);

        };
        removeButton.clickable.clicked += () =>
        {
            cheatCodeManager.CheckAndRemoveCheat(codeInput.value);
            ClearInput(codeInput);
        };
    }

    private void ClearInput(TextField textField)
    {
        textField.value = "";
    }
}