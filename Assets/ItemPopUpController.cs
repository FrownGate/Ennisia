using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPopUpController : MonoBehaviour
{
    [SerializeField] GameObject popup;  
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemPrice;
    [SerializeField] TextMeshProUGUI _itemDescription;
    [SerializeField] TextMeshProUGUI _availableAmount;
    
    [SerializeField] GameObject outsideClickArea;  
    [SerializeField] EventTrigger outsideClickAreaEventTrigger;
    private void Start()
    {
        outsideClickAreaEventTrigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ClosePopup(); });
        
        outsideClickAreaEventTrigger.triggers.Add(entry);
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
