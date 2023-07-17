using System.Collections.Generic;

public class Thief : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        targets[0].ApplyEffect(new BreakAttack());
        caster.ApplyEffect(new AttackBuff());
        return 0;
    }
}