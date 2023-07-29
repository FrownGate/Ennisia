using System;
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
    [SerializeField] TextMeshProUGUI _numberToBuy;
    [SerializeField] Image _itemImage;
    [SerializeField] Image _currencyImage;
    [SerializeField] CanvasGroup canvasGroup;
    
    private float _itemPriceFloat;
    
    public void OpenPopup()
    {
        popup.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    public void SetUpPopupInfo(int price, string name, string description)
    {
        _itemPriceFloat = price;
        
        _itemName.text = name;
        _itemPrice.text = price.ToString();
        _itemDescription.text = description;
        _numberToBuy.text = "1";
    }
    
    public void IncreaseNumberToBuy()
    {
        int amount = int.Parse(_numberToBuy.text);
        amount++;
        _numberToBuy.text = amount.ToString();
    }
    
    public void IncreaseTenNumberToBuy()
    {
        int amount = int.Parse(_numberToBuy.text);
        amount += 10;
        _numberToBuy.text = amount.ToString();
    }
    
    public void DecreaseNumberToBuy()
    {
        int amount = int.Parse(_numberToBuy.text);
        amount--;
        if (amount < 1)
        {
            amount = 1;
        }
        _numberToBuy.text = amount.ToString();
    }
    
    public void DecreaseTenNumberToBuy()
    {
        int amount = int.Parse(_numberToBuy.text);
        amount -= 10;
        if (amount < 1)
        {
            amount = 1;
        }
        _numberToBuy.text = amount.ToString();
    }
    
    
}
