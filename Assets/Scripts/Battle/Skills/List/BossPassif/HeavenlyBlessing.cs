using System.Collections.Generic;

public class HeavenlyBlessing : PassiveSkill
{
//TODO -> When Hp is lower than 30%, convert defense percentage gained by Heavenly Armor into Attack.
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AddAttack);
    }
    float AddAttack(float value) => value + Data.BuffAmount;
}
