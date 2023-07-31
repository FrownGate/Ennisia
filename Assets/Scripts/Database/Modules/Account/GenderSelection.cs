using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public partial class GenderSelection : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Dropdown(nameof(GetGendersValues))] private int _gender;
    
    private Vector3 initialScale;
    private Image image;
    
    void Start()
    {
        image = GetComponent<Image>();
        initialScale = this.transform.localScale;
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
        image.color = Color.yellow; // Change to the hover color.
        transform.localScale = initialScale * 1.1f;
        transform.position += new Vector3(0.1f, 0, 0);
    }

    public void OnHoverExit()
    {
        image.color = Color.white; // Change back to the normal color.
        transform.localScale = initialScale; 
        transform.position -= new Vector3(0.1f, 0, 0);
    }
}