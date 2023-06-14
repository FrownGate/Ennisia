using System;
using UnityEngine;

public class ShowPets : MonoBehaviour
{
    public static event Action<string> OnPopupShow;

    [SerializeField] private GameObject _prefabPetButton;
    [SerializeField] private GameObject _petPopup;
    [SerializeField] private GameObject _buttonsContainer;

    public Pets ActualPet;

    private Pets Pet;
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
        Type type = System.Type.GetType(CSVUtils.GetFileName(name));
        ActualPet = (Pets)Activator.CreateInstance(type);

        Debug.Log(ActualPet._name);
        OnPopupShow?.Invoke(name);
    }
}