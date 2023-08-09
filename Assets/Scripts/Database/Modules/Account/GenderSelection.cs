using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public partial class GenderSelection : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Dropdown(nameof(GetGendersValues))] private int _gender;
    [SerializeField] private GameObject _borderImage;
    private Vector3 initialScale;
    private Vector3 borderImageinitialScale;
    private Image image;
    
    void Start()
    {
        image = GetComponent<Image>();
        initialScale = this.transform.localScale;
        borderImageinitialScale = _borderImage.transform.localScale;
    }
    
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
        ScenesManager.Instance.SetScene(_mainMenu);
    }
    
    public void OnHover()
    {
        _borderImage.SetActive(true);
    }

    public void OnHoverExit()
    {
        _borderImage.SetActive(false);
    }
}