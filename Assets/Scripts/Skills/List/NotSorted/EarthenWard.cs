using System.Collections.Generic;

public class EarthenWard : ProtectionSkill
{
    private float _shieldAmount;
    //TODO -> Once every 2 turns, give a barrier scaling based off of max Hp.

    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    { 
        if (turn % 2 == 0) caster.Shield += (int)caster.Stats[Attribute.HP].Value * (20 / 100);
    }
}