using System;
using UnityEngine;

public class ClosePetPopup : MonoBehaviour
{
    public static event Action OnPetDiscard;

    private void OnMouseDown()
    {
        OnPetDiscard?.Invoke();
    }
}