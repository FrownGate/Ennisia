using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ShowShopBtnInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemPrice;
    [SerializeField] Image _itemImage;
    
    [HideInInspector]
    public int ItemId;
    [HideInInspector]
    public int ItemPrice;
    [HideInInspector]
    public string ItemName;
    [HideInInspector]
    public string ItemDescription;
    [HideInInspector]
    public Sprite ItemSprite;
    
    private ItemPopUpController _itemPopUpController => FindObjectOfType<ItemPopUpController>();
    void Start()
    {
        _itemName.text = ItemName;
        _itemPrice.text = ItemPrice.ToString();
        
    }

    private void OnMouseDown()
    {
        _itemPopUpController.SetUpPopupInfo(ItemPrice, ItemName, ItemDescription);
        _itemPopUpController.OpenPopup();
    }
}
