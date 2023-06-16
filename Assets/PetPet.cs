using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPet : MonoBehaviour
{
    public static event Action<string> OnUpdate;
    [SerializeField] private ShowPets _showPets;
    // Start is called before the first frame update
    public void OnMouseUp()
    {
        _showPets.ActualPet.Play();
        OnUpdate?.Invoke(_showPets.ActualPet._name);
    }
}
