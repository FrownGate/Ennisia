using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTips : MonoBehaviour
{


    [SerializeField] public List<string> tips;
    [SerializeField] private TextMeshProUGUI _tip;
    // Start is called before the first frame update
    void Start()
    {
        _tip.text = tips[Random.Range(1, tips.Count)];
    }


}
