using System;
using UnityEngine;

public class SkillHUD : MonoBehaviour
{
    public static event Action<Skill> OnSkillSelected;

    [SerializeField] private CanvasGroup _canvas;

    private Skill _skill;
    private bool _isActive;

    //TODO -> Add colors, cooldown numbers and popup

    public void Init(Skill skill, int x)
    {
        //TODO -> set sprite
        _skill = skill;
        transform.localPosition = new Vector3(-800 + x, -500, 0);
    }

    public void ToggleHUD(bool active)
    {
        _canvas.alpha = active ? 1 : 0;
        ToggleUse(active);
    }

    public void ToggleUse(bool active)
    {
        _isActive = active;
    }

    private void OnMouseUpAsButton()
    {
        if (!_isActive) return;
        //TODO -> use this event for sfx
        OnSkillSelected?.Invoke(_skill);
    }
}