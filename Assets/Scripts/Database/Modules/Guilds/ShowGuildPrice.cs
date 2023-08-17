using TMPro;
using UnityEngine;

public class ShowGuildPrice : MonoBehaviour
{
    [SerializeField] private TMP_Text _price;

    private void Awake()
    {
        _price.text = $"{PlayFabManager.Instance.GuildPrice} {Currency.Gold}";
    }
}