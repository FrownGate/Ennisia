using UnityEngine;
using UnityEngine.UI;

public class ShowSkillInfo : MonoBehaviour
{
    [SerializeField] private GameObject _skillInfo;

    public void OnMouseHover()
    {
        Debug.Log("Mouse Hover");
        _skillInfo.SetActive(true);
    }
    public void Update() 
    { 
        if (Input.GetMouseButton(0)) 
        {
            Debug.Log("Mouse Hold");
            _skillInfo.SetActive(true);
        }
    }
}