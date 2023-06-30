using System;
using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Editor.Scripts.Utility
{
    public struct GUIEnabledScope : IDisposable
    {
        public readonly bool PreviouslyEnabled;

        public GUIEnabledScope(bool enabled)
        {
            PreviouslyEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }

        public void Dispose()
        {
            GUI.enabled = PreviouslyEnabled;
        }
    }
}