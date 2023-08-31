using System.Collections.Generic;
using UnityEngine;

public class ShowSummonResult : MonoBehaviour
{
    [SerializeField] private GameObject _summonArea;
    [SerializeField] private GameObject _pulledSupportPrefab;

    private void Awake()
    {
        PlayFabManager.OnSummon += ShowSummon;
        PlayFabManager.Instance.Summon();
    }

    private void OnDestroy()
    {
        PlayFabManager.OnSummon -= ShowSummon;
    }

    private void ShowSummon(List<SupportCharacterSO> supports)
    {
        foreach (SupportCharacterSO character in supports)
        {
            var support = Instantiate(_pulledSupportPrefab, gameObject.transform);
            support.GetComponent<ShowSupport>().Init(character);
        }
    }
}