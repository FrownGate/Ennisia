using System.Collections.Generic;

public class How : BuffSkill
{
    
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            target.ApplyEffect(new AttackBuff());
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}