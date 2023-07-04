using System.Collections.Generic;

public class Reinforcement : PassiveSkill
{
    private float _defBaseRatio;
    //TODO -> finish skill

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        float defRatio = _defBaseRatio + StatUpgrade1 * Level;
        float PhdefBuff = caster.Stats[Attribute.PhysicalDefense].Value * defRatio;
        float MdefBuff = caster.Stats[Attribute.MagicalDefense].Value * defRatio;
    }
}