using TMPro;
using UnityEngine;

public class DisplayInformations : MonoBehaviour
{
    private PetSO _data;

    [SerializeField] public GameObject Image;
    [SerializeField] public GameObject Lore;
    [SerializeField] public GameObject Level;
    // Start is called before the first frame update

    void Awake()
    {
        OnClickPet.DisplayInformation += DisplayPet;
    }

    private void OnDestroy()
    {
        OnClickPet.DisplayInformation -= DisplayPet;
    }

    public void DisplayPet(string name)
    {
        Debug.Log(name);
        _data = Resources.Load<PetSO>("SO/Pets/" + name);
        //Image.GetComponent<Image>().sprite = _data.Icon;
        Lore.GetComponent<TextMeshProUGUI>().text = _data.Lore;
        Level.GetComponent<TextMeshProUGUI>().text = _data.Level.ToString();
    }
}