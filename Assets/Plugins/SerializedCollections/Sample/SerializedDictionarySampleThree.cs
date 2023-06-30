using Assets.Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Sample
{
    public class SerializedDictionarySampleThree : MonoBehaviour
    {
        [SerializeField]
        private SerializedDictionary<ScriptableObject, string> _nameOverrides;
    }
}