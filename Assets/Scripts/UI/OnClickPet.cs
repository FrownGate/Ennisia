using System;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class OnClickPet : MonoBehaviour
{
    public static event Action<string> DisplayInformation;
    public static event Action<string> Clear;

    [SerializeField] public GameObject Name;
    [SerializeField] public GameObject petInformation;
    [SerializeField] public GameObject Canvas;

    public void OnMouseDown()
    {
        if (Canvas.GetComponent<Pets>().InfoDisplayed == false)
        {
            GameObject tmp = Instantiate(petInformation, Canvas.transform);
            Canvas.GetComponent<Pets>().InfoDisplayed = true;
            tmp.name = Name.GetComponent<TextMeshProUGUI>().text;
        }
        else
        {

        }
        Debug.Log("clicked");
        DisplayInformation?.Invoke(Name.GetComponent<TextMeshProUGUI>().text);
    }
}