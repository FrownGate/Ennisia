using System.Collections.Generic;

public class Thief : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            target.ApplyEffect(new BreakAttack());
        }
        caster.ApplyEffect(new AttackBuff());
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}