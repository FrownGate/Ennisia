using System;
using UnityEngine;

public class ShowPets : MonoBehaviour
{
    public static event Action<string> OnPopupShow;

    [SerializeField] private GameObject _prefabPetButton;
    [SerializeField] private GameObject _petPopup;
    [SerializeField] private GameObject _buttonsContainer;

    private Pets Pet;
    private PetSO[] _petList;

    public void Awake()
    {

        Pet = new Yoichi();
        _petList = Resources.LoadAll<PetSO>("SO/Pets");

        PetButton.OnPetClick += ShowPetInfo;

        foreach (PetSO file in _petList)
        {
            GameObject currentButton = Instantiate(_prefabPetButton, transform.position, transform.rotation, _buttonsContainer.transform);
            currentButton.GetComponent<PetButton>().PetName = file.Name;
        }
    }

    private void OnDestroy()
    {
        PetButton.OnPetClick -= ShowPetInfo;
    }

    private void ShowPetInfo(string name)
    {
        if (!_petPopup.activeSelf) _petPopup.SetActive(true);
        OnPopupShow?.Invoke(name);
    }
}