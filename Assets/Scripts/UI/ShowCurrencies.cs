using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCurrencies : MonoBehaviour
{
    private GameObject _currencyPrefab;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _currencyPrefab = Resources.Load<GameObject>("Prefabs/UI/Currency");
        _rectTransform = GetComponent<RectTransform>();

        int position = 0;

        foreach (KeyValuePair<PlayFabManager.Currency, int> currency in PlayFabManager.Instance.Currencies)
        {
            Debug.Log($"{currency.Key} : {currency.Value}");

            GameObject currencyObject = Instantiate(_currencyPrefab, gameObject.transform);
            currencyObject.transform.localPosition = new Vector3((0 - _rectTransform.rect.width / 2.3f) + position, 0, 0);

            TextMeshProUGUI text = currencyObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = currency.Value.ToString(); //TODO -> crop currency amount if too big

            //TODO -> Change sprite depending on currency name

            position += 180;
        }
    }
}