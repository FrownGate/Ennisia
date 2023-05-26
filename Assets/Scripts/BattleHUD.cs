using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _lvlText;
    [SerializeField] private Slider _hpSlider;
    
    public void SetHUD(Entity target)
    {
        /*nameText.text = target.entityName;
        lvlText.text = target.level.ToString();*/
        _hpSlider.maxValue = target.MaxHp;
        _hpSlider.value = target.CurrentHp;
    }

    public void SetHp(float hp)
    {
        _hpSlider.value = hp;
    }
}