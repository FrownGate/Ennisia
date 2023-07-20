using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowShopInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _shopName;
    
    public string ShopName { get; set; } = "Shop Name";
    void Start()
    {
        _shopName.text = ShopName;
    }

}
