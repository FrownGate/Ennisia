using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTips : MonoBehaviour
{
    //TODO -> use CSV instead of serialize field
    [SerializeField] private List<string> _tips;
    [SerializeField] private TextMeshProUGUI _tip;

    void Start()
    {
        _tip.text = _tips[Random.Range(1, _tips.Count)];
    }
}