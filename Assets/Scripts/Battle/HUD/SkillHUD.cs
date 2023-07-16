using UnityEngine;

public class SkillHUD : HUD
{
    //TODO -> Add colors, cooldown numbers and popup

    public virtual void Init(Skill skill, int x)
    {
        //TODO -> set sprite
        _skill = skill;
        transform.localPosition = new Vector3(-800 + x, -465, 0);
    }
}