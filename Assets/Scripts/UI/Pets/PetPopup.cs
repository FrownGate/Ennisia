using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetPopup : MonoBehaviour
{
    private PetSO _data;

    [SerializeField] private Image _image;
    [SerializeField] private ShowPets _showpet;
    [SerializeField] private TextMeshProUGUI _lore;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _xp;
    [SerializeField] private Slider _gauge;

    void Awake()
    {
        PetPet.OnUpdate += DisplayPet;
        ShowPets.OnPopupShow += DisplayPet;
        ClosePetPopup.OnPetDiscard += Close;
    }

    private void OnDestroy()
    {
        PetPet.OnUpdate += DisplayPet;
        ShowPets.OnPopupShow -= DisplayPet;
        ClosePetPopup.OnPetDiscard -= Close;
    }

    private void DisplayPet(string name)
    {
        _data = Resources.Load<PetSO>("SO/Pets/" + name);
        //Image.sprite = _data.Icon;
        _lore.text = _data.Lore;
        _level.text = _showpet.ActualPet.AffinityLevel.ToString();
        _gauge.maxValue = _showpet.ActualPet.ToGetXp;
        _gauge.value = _showpet.ActualPet.ActualXP;
        _xp.text = _showpet.ActualPet.ActualXP.ToString() + " / " + _showpet.ActualPet.ToGetXp.ToString();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}