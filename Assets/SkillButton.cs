using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : HUD
{

    [SerializeField] protected Image _image;
    [SerializeField] protected Image _bgImagemage;
    
    public virtual void Init(Skill skill, int x)
    {
        _image.sprite = skill.Data.Icon != null ? skill.Data.Icon : _blankSprite;
        _skill = skill;
        transform.localPosition = new Vector3(-800 + x, -465, 0);
    }
}
