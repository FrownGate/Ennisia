using System;
using UnityEngine;

public class ShowPets : MonoBehaviour
{
    public static event Action<string> OnPopupShow;

    [SerializeField] private GameObject _prefabPetButton;
    [SerializeField] private GameObject _petPopup;
    [SerializeField] private GameObject _buttonsContainer;

    public Pet ActualPet { get; private set; }
    private PetSO[] _petList;

    public void Awake()
    {
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
        Type type = Type.GetType(CSVUtils.GetFileName(name));
        ActualPet = (Pet)Activator.CreateInstance(type);

        Debug.Log(ActualPet._name);
        OnPopupShow?.Invoke(name);
    }
}