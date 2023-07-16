using System;
using TMPro;
using UnityEngine;

public class PetButton : MonoBehaviour
{
    public static event Action<string> OnPetClick;

    [SerializeField] private TextMeshProUGUI _petName;

    public string PetName;

    private void Start()
    {
        _petName.text = PetName;
    }

    public void OnMouseDown()
    {
        OnPetClick?.Invoke(PetName);
    }
}