using System.Collections.Generic;
using UnityEngine;

public class SupportHUD : HUD
{
    private List<Skill> _skills;

    public void Init(List<Skill> skills, int y)
    {
        transform.localPosition = new Vector3(-850, 50 + y, 0);

        if (skills == null)
        {
            _sprite.sprite = _blankSprite;
            return;
        }

        _skills = skills;

        foreach (Skill skill in _skills)
        {
            if (!skill.Data.IsPassive) _skill = skill;
            break;
        }
    }
}