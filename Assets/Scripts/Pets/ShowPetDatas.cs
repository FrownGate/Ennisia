using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPetDatas : MonoBehaviour
{
    private PetSO _data;

    [SerializeField] private Image _image;
    [SerializeField] private ShowAllPets _showpet;
    [SerializeField] private TextMeshProUGUI _lore;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _xp;
    [SerializeField] private Slider _gauge;
    [SerializeField] private TextMeshProUGUI _nonObtainedText;
    [SerializeField] private TextMeshProUGUI _howToText;
    [SerializeField] private List<GameObject> _buttons;

    void Awake()
    {
        PetPet.OnUpdate += DisplayPet;
        ShowAllPets.OnPopupShow += DisplayPet;
        ShowAllPets.OnNonObtained += DisplayNotObtained;
        ClosePetPopup.OnPetDiscard += Close;
    }

    private void OnDestroy()
    {
        PetPet.OnUpdate += DisplayPet;
        ShowAllPets.OnPopupShow -= DisplayPet;
        ShowAllPets.OnNonObtained -= DisplayNotObtained;
        ClosePetPopup.OnPetDiscard -= Close;
    }

    private void DisplayPet(string name)
    {
        _lore.enabled = true;
        _level.enabled = true;
        _gauge.enabled = true;

        _nonObtainedText.enabled = false;
        _howToText.enabled = false;

        _data = Resources.Load<PetSO>("SO/Pets/" + name);
        //Image.sprite = _data.Icon;
        _lore.text = _data.Lore;
        _level.text = _showpet.ActualPet.AffinityLevel.ToString();
        _gauge.maxValue = _showpet.ActualPet.ToGetXp;
        _gauge.value = _showpet.ActualPet.ActualXP;
        _xp.text = _showpet.ActualPet.ActualXP.ToString() + " / " + _showpet.ActualPet.ToGetXp.ToString();

        foreach (GameObject button in _buttons)
        {
            button.SetActive(true);
        }
    }
    private void DisplayNotObtained(string name)
    {
        Debug.Log("not obtained");
        _image.enabled = false;
        _nonObtainedText.enabled = true;
        _howToText.enabled = true;
        _data = Resources.Load<PetSO>("SO/Pets/" + name);
        _lore.enabled = false;
        _level.enabled = false;
        _gauge.enabled = false;
        _xp.enabled = false;

        foreach(GameObject button in _buttons)
        {
            button.SetActive(false);
        }

        // _howToText.text = _data.HowToObtain


    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}