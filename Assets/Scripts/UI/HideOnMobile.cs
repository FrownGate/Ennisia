using UnityEngine;

public class HideOnMobile : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
        void Awake( ) 
        { 
            gameObject.SetActive(false);
        }
#endif
}