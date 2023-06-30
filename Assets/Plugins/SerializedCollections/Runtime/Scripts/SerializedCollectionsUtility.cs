﻿using UnityEngine;

namespace Assets.Plugins.SerializedCollections.Runtime.Scripts
{
    public static class SerializedCollectionsUtility
    {
        public static bool IsValidKey(object obj)
        {
            // we catch this error if we are not on the main thread and simply return false as we assume the object is null
            try
            {
                return !(obj == null || (obj is Object unityObject && unityObject == null));
            }
            catch
            {
                return false;
            }
        }

        public static bool KeysAreEqual<T>(T key, object otherKey)
        {
            return (object)key == otherKey || key.Equals(otherKey);
        }
    }
}