using System.Collections.Generic;
using UnityEngine;

public class SupportHUD : HUD
{
    private List<Skill> _skills;

    public void Init(SupportCharacterSO support, int y)
    {
        transform.localPosition = new Vector3(-850, 50 + y, 0);

        
        if(support != null) _image.sprite = support.Icon != null ? support.Icon : _blankSprite;
        
        
        var skills = support == null ? null : support.Skills;
        
        if (skills == null)
        {
            ToggleHUD(false);
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