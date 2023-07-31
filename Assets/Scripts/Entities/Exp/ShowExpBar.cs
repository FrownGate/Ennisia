using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowExpBar : MonoBehaviour
{
    public bool AccountBar;
    public bool PlayerBar;
    private TextMeshProUGUI _expBarText;
    private Slider _expBarSlider;
    private int _level = 0;
    private int _exp = 0;

    public void Update()
    {
        _expBarSlider = GetComponent<Slider>();
        _expBarText = GetComponentInChildren<TextMeshProUGUI>();

        if (AccountBar)
        {
            _level = PlayFabManager.Instance.Account.Level;
            _exp = PlayFabManager.Instance.Account.Exp;
            _expBarSlider.maxValue = ExpManager.Instance.AccountlevelExperienceMap[_level + 1];
        }
        else if (PlayerBar)
        {
            _level = PlayFabManager.Instance.Player.Level;
            _exp = PlayFabManager.Instance.Player.Exp;
            _expBarSlider.maxValue = PlayFabManager.Instance.Player.PlayerlevelExperienceMap[_level + 1];
        }

        _expBarSlider.value = _exp;
        _expBarText.text = "lvl " + _level;
    }
}