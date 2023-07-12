using System.Collections.Generic;

public class Charge : DamageSkill
{ 
    private float _attackBaseRatio;
    private float _attackRatioBuff; //Used ?
    
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _attackRatioBuff = _attackBaseRatio + StatUpgrade1 * Level;
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AddAttackBuff);
    }

    float AddAttackBuff(float value) => _attackBaseRatio + value;
}