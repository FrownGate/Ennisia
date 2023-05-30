using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{

    public class HideOnPc : MonoBehaviour
    {

#if UNITY_IOS || UNITY_ANDROID
        void Awake( ) 
        { 
            gameObject.SetActive(false);
        }
#endif
    }
}