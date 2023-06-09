using System;
using TMPro;
using UnityEngine;

public class OnClickPet : MonoBehaviour
{
    public static event Action<string> DisplayInformation;

   [SerializeField] public GameObject Name;
   [SerializeField] public GameObject petInformation;
   [SerializeField] public GameObject Canvas;

   public void OnMouseDown()
    {
        Debug.Log("clicked");
        Instantiate(petInformation, Canvas.transform);
        DisplayInformation?.Invoke(Name.GetComponent<TextMeshProUGUI>().text);
    }
}