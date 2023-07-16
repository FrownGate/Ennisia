using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowBanner : MonoBehaviour
{
    public static event Action<SummonBannerSO> OnClick;

    [SerializeField] private Image _image;

    private SummonBannerSO _banner;

    public void Init(SummonBannerSO banner)
    {
        //TODO -> set image
        _banner = banner;
    }

    private void OnMouseUpAsButton()
    {
        if (_banner == null) return;
        OnClick?.Invoke(_banner);
    }
}