using System.Collections.Generic;

public class GalebladeStrike : DamageSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.ApplyEffect(new AttackBuff(2));       
    }
}