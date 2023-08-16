using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _blankSprite;
    [SerializeField] private Image _bgImagemage;
    [SerializeField] private Image _frameImage;  
    public static event Action<Skill> OnSkillSelected;
    
    private Skill _skill;
    private bool _isActive;
    private bool _isSelected;
    
    private float _rotateSpeed => 0.5f;
    private void Update()
    {
        
        if (_isSelected)
        {
            _bgImagemage.transform.Rotate(Vector3.forward, _rotateSpeed);
        }
    }

    public void ToggleHUD(bool active)
    {
        if (active && !_skill.IsUseable()) return;
        //_newframeImage.sprite = active ? 1 : 0;
        ToggleUse(active);
    }

    public void ToggleUse(bool active)
    {
        if (active && !_skill.IsUseable()) return;
        _isActive = active;
    }

    public void OnMouseUpAsButton()
    {
        //if (!_isActive || _skill == null) return;
        _isSelected = true;
        OnSkillSelected?.Invoke(_skill);
    }

    public void OnMouseOver()
    {
        _frameImage.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        _frameImage.gameObject.SetActive(false);
    }
    
    public void Init(Skill skill)
    {
        _image.sprite = skill.Data.Icon != null ? skill.Data.Icon : _blankSprite;
        _skill = skill;
    }
}
