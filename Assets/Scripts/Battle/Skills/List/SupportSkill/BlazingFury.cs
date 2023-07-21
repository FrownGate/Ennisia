using System.Collections.Generic;

public class BlazingFury : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new AttackBuff(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}