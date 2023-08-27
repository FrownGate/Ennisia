using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowAllPets : MonoBehaviour
{
    public static event Action<string> OnPopupShow;
    public static event Action<string> OnNonObtained;

    [SerializeField] private GameObject _prefabPetButton;
    [SerializeField] private GameObject _petPopup;
    [SerializeField] private GameObject _buttonsContainer;

    public Dictionary<string, Pet> AllPets;

    public Pet ActualPet { get; private set; }
    private PetSO[] _petList;

    public void Awake()
    {
        AllPets = new Dictionary<string, Pet>();
        _petList = Resources.LoadAll<PetSO>("SO/Pets");

        ShowPetButton.OnPetClick += ShowPetInfo;

        foreach (PetSO file in _petList)
        {
            GameObject currentButton = Instantiate(_prefabPetButton, transform.position, transform.rotation, _buttonsContainer.transform);
            currentButton.GetComponent<ShowPetButton>().PetName = file.Name;
            Type type = Type.GetType(CSVUtils.GetFileName(file.Name));
            Debug.Log(file.Name);
            Debug.Log(type);
            AllPets.Add(file.Name, (Pet)Activator.CreateInstance(type));
        }
    }

    private void OnDestroy()
    {
        ShowPetButton.OnPetClick -= ShowPetInfo;
    }

    private void ShowPetInfo(string name)
    {
        if (!_petPopup.activeSelf) _petPopup.SetActive(true);
        ActualPet = AllPets[name];
        if (ActualPet.Obatined)
        {
            
            OnPopupShow?.Invoke(name);
        }
        else
        {
           
            OnNonObtained?.Invoke(name);
        }

    }
}