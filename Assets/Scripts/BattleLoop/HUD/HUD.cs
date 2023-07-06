using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static event Action<Skill> OnSkillSelected;

    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected CanvasGroup _canvas;

    protected Skill _skill;
    protected bool _isActive;

    public void ToggleHUD(bool active)
    {
        _canvas.alpha = active ? 1 : 0;
        ToggleUse(active);
    }

    public void ToggleUse(bool active)
    {
        _isActive = active;
    }

    protected void OnMouseUpAsButton()
    {
        if (!_isActive || _skill == null) return;
        OnSkillSelected?.Invoke(_skill);
    }
}