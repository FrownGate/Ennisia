using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static event Action<Skill> OnSkillSelected;

    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected CanvasGroup _canvas;
    [SerializeField] protected Sprite _blankSprite;

    protected Skill _skill;
    protected bool _isActive;

    public void ToggleHUD(bool active)
    {
        if (active && !_skill.IsUseable()) return;
        _canvas.alpha = active ? 1 : 0;
        ToggleUse(active);
    }

    public void ToggleUse(bool active)
    {
        if (active && !_skill.IsUseable()) return;
        _isActive = active;
    }

    protected void OnMouseUpAsButton()
    {
        if (!_isActive || _skill == null) return;
        OnSkillSelected?.Invoke(_skill);
    }
}