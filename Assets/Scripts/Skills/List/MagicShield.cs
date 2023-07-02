using System.Collections.Generic;

public class MagicShield : Skill
{
    private float _magicDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(MagicShieldBuff);
    }

    float MagicShieldBuff(float input) => input * (1 + _magicDefBuffRatio);
}