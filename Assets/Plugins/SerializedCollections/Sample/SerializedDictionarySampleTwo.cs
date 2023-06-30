using Assets.Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Sample
{
    public class SerializedDictionarySampleTwo : MonoBehaviour
    {
        [SerializedDictionary("ID", "Person")]
        public SerializedDictionary<int, Person> People;

        [System.Serializable]
        public class Person
        {
            public string FirstName;
            public string LastName;
        }
    }
}