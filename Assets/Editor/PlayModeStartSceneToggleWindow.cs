using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayModeStartSceneToggleWindow : EditorWindow
{
    private bool activatePlayModeStartScene = true;

    [MenuItem("PlayMode/Play Mode Start Scene Toggle")]
    public static void ShowWindow()
    {
        GetWindow<PlayModeStartSceneToggleWindow>("Play Mode Start Scene Toggle");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Play Mode Start Scene Toggle", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        activatePlayModeStartScene = EditorGUILayout.Toggle("Activate Play Mode Start Scene", activatePlayModeStartScene);

        if (GUILayout.Button("Apply"))
        {
            ApplyPlayModeStartSceneToggle();
        }
    }

    private void ApplyPlayModeStartSceneToggle()
    {
        string scenePath = "Assets/Scenes/TitleScreen.unity";
        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

        if (sceneAsset != null)
        {
            if (activatePlayModeStartScene)
            {
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
            else
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
        else
        {
            Debug.LogError("Failed to load the title screen scene: " + scenePath);
        }
    }
}
