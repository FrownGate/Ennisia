using NaughtyAttributes;
using UnityEngine;

public class GenderSelection : MonoBehaviour
{
    [SerializeField, Dropdown(nameof(GetGendersValues))] private int _gender;

    private DropdownList<int> GetGendersValues()
    {
        return new()
        {
            { "Female", 1 },
            { "Male", 2 }
        };
    }

    private void OnMouseDown()
    {
        PlayFabManager.Instance.SetGender(_gender);
    }
}