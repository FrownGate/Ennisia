using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityEngine.Object), true)]
public class DictionaryInspector : Editor
{
    private Dictionary<string, SerializedProperty> dictionaryProperties = new Dictionary<string, SerializedProperty>();

    protected virtual void OnEnable()
    {
        SerializedProperty iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue is ScriptableObject)
            {
                var scriptableObject = (ScriptableObject)iterator.objectReferenceValue;
                var scriptableObjectProperties = new SerializedObject(scriptableObject).GetIterator();
                while (scriptableObjectProperties.NextVisible(true))
                {
                    if (scriptableObjectProperties.propertyType == SerializedPropertyType.Generic &&
                        scriptableObjectProperties.type.Contains("Dictionary"))
                    {
                        dictionaryProperties[scriptableObjectProperties.displayName] = scriptableObjectProperties.Copy();
                    }
                }
            }
            else if (iterator.propertyType == SerializedPropertyType.Generic &&
                     iterator.type.Contains("Dictionary"))
            {
                dictionaryProperties[iterator.displayName] = iterator.Copy();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Dictionary Values:");

        foreach (var kvp in dictionaryProperties)
        {
            EditorGUILayout.PropertyField(kvp.Value);
        }

        serializedObject.ApplyModifiedProperties();
    }
}