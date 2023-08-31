using UnityEngine;
using System.Collections.Generic;
using System;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Instance {  get; private set; }

    private SummonBannerSO _summonBanner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        ShowBanner.OnClick += SetBanner;
    }

    private void OnDestroy()
    {
        ShowBanner.OnClick -= SetBanner;
    }

    private void SetBanner(SummonBannerSO banner)
    {
        _summonBanner = banner;
    }
}