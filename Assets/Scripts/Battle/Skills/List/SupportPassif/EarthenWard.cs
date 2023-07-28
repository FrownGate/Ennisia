using System.Collections.Generic;

public class EarthenWard : ProtectionSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    { 
        if (turn % 2 == 0) caster.Shield += caster.Stats[Attribute.HP].Value * (Data.ShieldAmount / 100);
    }
}