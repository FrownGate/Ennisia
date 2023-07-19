using System.Collections.Generic;

public class RisingPower : PassiveSkill
{

    private float _attackBuff;
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        float attackBuffRatio = (Data.BuffAmount/100) + (StatUpgrade1 * Level);
       
        if (caster.Weapon.Type != 0)
        {
            _attackBuff = caster.Stats[Attribute.Attack].Value * attackBuffRatio;
        }else
        {
            _attackBuff = caster.Stats[Attribute.Attack].Value * attackBuffRatio * 2;     
        }
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.PhysicalDefense].AddModifier(AtkBuff);
    }
    float AtkBuff(float value) => value + _attackBuff;
}