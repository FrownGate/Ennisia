using System.Collections.Generic;

public class FlawlessTechnique : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.CritRate] = caster.Stats[Attribute.CritRate].AddModifier(AddCritRate);
    }

    float AddCritRate(float value) => value + 15;

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        if (targets[0].IsDead)
        {
            //give attack buff
        }
    }
}