using System.Collections.Generic;

public class Howl : BuffSkill
{
    
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            target.ApplyEffect(new AttackBuff());
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}