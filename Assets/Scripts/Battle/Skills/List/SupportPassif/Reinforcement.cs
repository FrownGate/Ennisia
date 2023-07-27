using System.Collections.Generic;

public class Reinforcement : PassiveSkill
{
    float PhdefBuff;
    float MdefBuff;
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        float defRatio = (Data.BuffAmount/100) + StatUpgrade1 * Level;
        PhdefBuff = caster.Stats[Attribute.PhysicalDefense].Value * defRatio;
        MdefBuff = caster.Stats[Attribute.MagicalDefense].Value * defRatio;
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(PHDefBuff);
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(MDefBuff);
    }

    float PHDefBuff(float value) => value + PhdefBuff;

    float MDefBuff(float value) => value + MdefBuff;
}