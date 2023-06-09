using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetPopup : MonoBehaviour
{
    private PetSO _data;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _lore;
    [SerializeField] private TextMeshProUGUI _level;

    void Awake()
    {
        ShowPets.OnPopupShow += DisplayPet;
        ClosePetPopup.OnPetDiscard += Close;
    }

    private void OnDestroy()
    {
        ShowPets.OnPopupShow -= DisplayPet;
        ClosePetPopup.OnPetDiscard -= Close;
    }

    private void DisplayPet(string name)
    {
        _data = Resources.Load<PetSO>("SO/Pets/" + name);
        //Image.sprite = _data.Icon;
        _lore.text = _data.Lore;
        _level.text = _data.Level.ToString();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}