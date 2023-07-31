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
        image.color = new Color(0.16f, 0.49f, 1f); //DELETE IT FOR BUILD 
        transform.localScale = initialScale * 1.2f;
        _borderImage.transform.localScale = borderImageinitialScale * 1.1f;
        _borderImage.SetActive(true);
    }

    public void OnHoverExit()
    {
        image.color = Color.white;
        transform.localScale = initialScale;
        _borderImage.transform.localScale = borderImageinitialScale;
        _borderImage.SetActive(false);
    }
}