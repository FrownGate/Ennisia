using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ItemPopUpController : MonoBehaviour
{
    //Pop Up
    [SerializeField] GameObject popup;  
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemPrice;
    [SerializeField] TextMeshProUGUI _itemDescription;
    [SerializeField] TextMeshProUGUI _availableAmount;
    [SerializeField] Image _itemImage;
    [SerializeField] Image _currencyImage;
    
    [SerializeField] GameObject outsideClickArea;  
    [SerializeField] EventTrigger outsideClickAreaEventTrigger;
    private void Start()
    {
        outsideClickAreaEventTrigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { ClosePopup(); });
        
        outsideClickAreaEventTrigger.triggers.Add(entry);
    }
    void Update () 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) 
            {
                if (hit.transform == outsideClickArea.transform) 
                {
                    popup.SetActive(false);
                }
            }
        }
    }

    public void OpenPopup()
    {
        popup.SetActive(true);
        outsideClickArea.SetActive(true);
    }

    private void ClosePopup()
    {
        popup.SetActive(false);
        outsideClickArea.SetActive(false);
    }
    
    public void SetUpPopupInfo(int price, string name, string description)
    {
        _itemName.text = name;
        _itemPrice.text = price.ToString();
        _itemDescription.text = description;
    }
}
