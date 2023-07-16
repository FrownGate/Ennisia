using System;
using UnityEngine;

public class PetPet : MonoBehaviour
{
    public static event Action<string> OnUpdate;
    [SerializeField] private ShowAllPets _showPets;

    public void OnMouseUpAsButton()
    {
        _showPets.ActualPet.Play();
        OnUpdate?.Invoke(_showPets.ActualPet._name);
    }
}