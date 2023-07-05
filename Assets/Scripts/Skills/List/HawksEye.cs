using System.Collections.Generic;

public class HawksEye : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new CritRateBuff());
        caster.ApplyEffect(new CritDmgBuff());
        caster.ApplyEffect(new AttackBuff());

        return 0;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.AtkBarPercentage = 100;
    }
}