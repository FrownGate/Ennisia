using System.Collections;
using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Editor.Scripts.KeyListGenerators
{
    public abstract class KeyListGenerator : ScriptableObject
    {
        public abstract IEnumerable GetKeys(System.Type type);
    }
}