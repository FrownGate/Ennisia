using System.Collections.Generic;
using UnityEngine;

public class Currencies : MonoBehaviour
{
    private void Awake()
    {
        foreach (KeyValuePair<string, int> currency in PlayFabManager.Instance.Currencies)
        {
            Debug.Log($"{currency.Key} : {currency.Value}");
            //TODO -> Create instance of Currency prefab
        }
    }
}