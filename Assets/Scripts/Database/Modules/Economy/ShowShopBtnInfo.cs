using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowShopBtnInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemPrice;
    
    [HideInInspector]
    public int ItemId;
    [HideInInspector]
    public int ItemPrice;
    [HideInInspector]
    public string ItemName;
    [HideInInspector]
    public string ItemDescription;
    
    void Start()
    {
        _itemName.text = ItemName;
        _itemPrice.text = ItemPrice.ToString();
    }
    
    
}
