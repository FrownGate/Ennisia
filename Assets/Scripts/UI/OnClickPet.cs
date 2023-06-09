using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OnClickPet : MonoBehaviour
{
    public static event Action<string> DisplayInformation;

   [SerializeField] public GameObject name;
   [SerializeField] public GameObject petInformation;
   [SerializeField] public GameObject Canvas;
    // Start is called before the first frame update
   public void OnMouseDown()
    {
        Debug.Log("clicked");
        Instantiate(petInformation, Canvas.transform);
        DisplayInformation?.Invoke(name.GetComponent<TextMeshProUGUI>().text);
    }
}
    