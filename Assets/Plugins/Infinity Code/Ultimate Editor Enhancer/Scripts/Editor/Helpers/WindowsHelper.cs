﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.UltimateEditorEnhancer.Integration;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class WindowsHelper
    {
        public const string MenuPath = "Tools/Infinity Code/Ultimate Editor Enhancer/";

        public static void ShowInspector()
        {
            Event e = Event.current;
            Type windowType = InspectorWindowRef.type;

            Vector2 size = Prefs.defaultWindowSize;
            Rect rect = new Rect(GUIUtility.GUIToScreenPoint(e.mousePosition) - size / 2, Vector2.zero);

            Rect windowRect = new Rect(rect.position, size);
            EditorWindow window = ScriptableObject.CreateInstance(windowType) as EditorWindow;
            window.Show();
            window.position = windowRect;
        }

        public static bool IsMaximized(EditorWindow window)
        {
            if (window == null) return false;
            if (FullscreenEditor.IsFullscreen(window)) return true;
            return window.maximized;
        }

        public static void SetMaximized(EditorWindow window, bool maximized)
        {
            if (window == null) return;

            bool state = IsMaximized(window);
            if (state == maximized) return;

            bool logState = Debug.unityLogger.logEnabled;
            Debug.unityLogger.logEnabled = false;
            window.maximized = maximized;
            Debug.unityLogger.logEnabled = logState;
        }

        public static void SetMaximizedOrFullscreen(EditorWindow window, bool maximized)
        {
            if (window == null) return;

            bool state = IsMaximized(window);
            if (state == maximized) return;

            bool logState = Debug.unityLogger.logEnabled;
            Debug.unityLogger.logEnabled = false;

            if (maximized) window.maximized = true;
            else
            {
                if (FullscreenEditor.IsFullscreen(window)) FullscreenEditor.ToggleFullscreen(window);
                window.maximized = false;
            }

            Debug.unityLogger.logEnabled = logState;
        }

        public static void SetRect(EditorWindow window, Rect rect)
        {
            window.position = rect;
            Rect r = window.position;
            if (Math.Abs(r.width - rect.width) < 1 && Math.Abs(r.height - rect.height) < 1) return;

            var parent = EditorWindowRef.GetParent(window);
            r = HostViewRef.GetPosition(parent);
            r.width = rect.width;
            r.height = rect.height;

            ViewRef.SetMinMaxSizes(parent, Vector2.zero, new Vector2(4000, 4000));
            HostViewRef.SetPosition(parent, r);
        }

        public static void ToggleMaximized(EditorWindow window)
        {
            SetMaximizedOrFullscreen(window, !IsMaximized(window));
        }
    }
}