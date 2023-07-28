using System.Collections.Generic;

public class Charge : DamageSkill
{ 

    
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AddAttackBuff);
    }

    float AddAttackBuff(float value) => ((Data.BuffAmount / 100) + StatUpgrade1 * Level) * value;
}