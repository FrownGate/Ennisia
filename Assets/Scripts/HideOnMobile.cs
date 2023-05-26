using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{

    public class HideOnMobile : MonoBehaviour
    {

#if UNITY_STANDALONE
        void Awake( ) 
        { 
            gameObject.SetActive(false);
        }
#endif
    }
}