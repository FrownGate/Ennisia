using UnityEngine;

public class HideOnPc : MonoBehaviour
{
#if UNITY_STANDALONE
    void Awake()
    {
        gameObject.SetActive(false);
    }
#endif
}