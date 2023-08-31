using System;
using UnityEngine;

public class ShowSupportData : MonoBehaviour
{
    public static event Action<SupportCharacterSO> OnClick;

    public SupportCharacterSO Support;

    public void Click()
    {
        OnClick?.Invoke(Support);
    }
}