using System.Collections.Generic;
using UnityEngine;

public class SummonPopup : MonoBehaviour
{
    [SerializeField] private GameObject _summonArea;
    [SerializeField] private GameObject _pulledSupportPrefab;

    private void Awake()
    {
        PlayFabManager.OnSummon += Init;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnSummon -= Init;
    }

    private void Init(List<SupportCharacterSO> pulledSupports)
    {
        Debug.Log(pulledSupports.Count);
        int row = 0;
        int pos = 150;

        for (int i = 0; i < pulledSupports.Count; i++)
        {
            //row = i % 5 == 0 ? row + 1 : row;
            //pos = i % 5 == 0 ? 0 : pos + 190;
            //Vector3 position = new(500 + pos, 300 + 190 * row, 3);

            GameObject pulledSupport = Instantiate(_pulledSupportPrefab, _summonArea.transform);
            //pulledSupport.transform.position = position;
            pulledSupport.GetComponent<ShowSupport>().Init(pulledSupports[i]);
        }
    }
}