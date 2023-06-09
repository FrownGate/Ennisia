using UnityEngine;
using UnityEngine.UI;

public class EntityController : MonoBehaviour
{
    public Entity Entity { get; protected set; }
    private Slider _hpBar;

    private void Awake()
    {
        InitEntity();
        InitHUD();
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void InitHUD()
    {
        _hpBar = GetComponentInChildren<Slider>();
        _hpBar.maxValue = Entity.MaxHp;
        _hpBar.value = Entity.CurrentHp;
    }

    private void UpdateHUD()
    {
        _hpBar.value = Entity.CurrentHp >= 0 ? Entity.CurrentHp : 0;
    }

    public virtual void InitEntity() { }
}