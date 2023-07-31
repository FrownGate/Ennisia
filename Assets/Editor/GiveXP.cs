using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GiveXP : EditorWindow
{
    private int _value;

    [MenuItem("Tools/GiveXP")]
    public static void ShowWindow()
    {
        GetWindow<GiveXP>("GiveXP");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Give PlayerXP", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        _value = EditorGUILayout.IntField("Xp to add", _value);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("To Account")) ExpManager.Instance.GainExperienceAccount(_value);
        if (GUILayout.Button("To Player")) PlayFabManager.Instance.Player.GainExperiencePlayer(_value);
        EditorGUILayout.EndHorizontal();
    }
}