using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowShopInfo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI _shopName;
    
    private ShopSystem _shopSystem => FindObjectOfType<ShopSystem>();
    public string ShopName { get; set; } = "Shop Name";
    void Start()
    {
        _shopName.text = ShopName;
    }

    private void OnMouseUpAsButton()
    {
        _shopSystem.OnShopBtnClick(ShopName);
        Debug.Log("Clicked");
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        _shopSystem.OnShopBtnClick(ShopName);
    }
}
