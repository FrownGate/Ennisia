﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public class TemporaryContainer : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
            Destroy(gameObject);
#endif
        }

        public static GameObject GetContainer()
        {
            TemporaryContainer temporaryContainer = FindObjectOfType<TemporaryContainer>();
            if (temporaryContainer == null)
            {
                GameObject go = new GameObject("Temporary Container");
                go.tag = "EditorOnly";
                temporaryContainer = go.AddComponent<TemporaryContainer>();
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Temporary Container");
                }
#endif
            }
            return temporaryContainer.gameObject;
        }
    }
}