using System.Collections.Generic;

public class SharpeningSword : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.ApplyEffect(new AttackBuff(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}