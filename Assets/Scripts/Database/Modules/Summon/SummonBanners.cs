using UnityEngine;

public class SummonBanners : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _bannerPrefab;

    private SummonBannerSO[] _banners;

    private void Awake()
    {
        _banners = Resources.LoadAll<SummonBannerSO>("SO/SummonBanners");

        for (int i = 0; i < _banners.Length; i++)
        {
            GameObject newBanner = Instantiate(_bannerPrefab, _content.transform);
            newBanner.GetComponent<ShowBanner>().Init(_banners[i]);
        }
    }
}