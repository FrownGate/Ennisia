using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowShopInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _shopName;
    
    public string ShopName { get; set; }
    void Start()
    {
        _shopName.text = ShopName;
    }

}
