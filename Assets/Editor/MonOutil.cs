using UnityEditor;
using UnityEngine;

public class MonOutil : EditorWindow
{
    private int selectedTab = 0;
    private GUIContent[] tabLabels = new GUIContent[]
    {
        new GUIContent("Onglet 1"),
        new GUIContent("Onglet 2")
    };

    [MenuItem("Tools/Mon Outil")]
    public static void ShowWindow()
    {
        GetWindow<MonOutil>("Mon Outil");
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabLabels);

        switch (selectedTab)
        {
            case 0:
                // Contenu de l'onglet 1
                GUILayout.Label("Contenu de l'onglet 1");
                break;

            case 1:
                // Contenu de l'onglet 2
                GUILayout.Label("Contenu de l'onglet 2");
                break;
        }
    }
}
