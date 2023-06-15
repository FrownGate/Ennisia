using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(QuestSO))]
public class QuestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestSO script = (QuestSO)target;

        // Check if the script has a dictionary
        if (script.currencyList != null)
        {
            Debug.Log("Penetre");

            // Display the dictionary values
            foreach (KeyValuePair<PlayFabManager.Currency, int> pair in script.currencyList)
            {
                Debug.Log(pair.Value.ToString());
                EditorGUILayout.LabelField(pair.Key.ToString(), pair.Value.ToString());
            }
        }
    }
}
