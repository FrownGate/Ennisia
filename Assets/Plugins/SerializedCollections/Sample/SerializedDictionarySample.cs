using Assets.Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Sample
{
    public class SerializedDictionarySample : MonoBehaviour
    {
        [SerializedDictionary("Element Type", "Description")]
        public SerializedDictionary<ElementType, string> ElementDescriptions;
        
        public enum ElementType
        {
            Fire,
            Air,
            Earth,
            Water
        }
    }
}