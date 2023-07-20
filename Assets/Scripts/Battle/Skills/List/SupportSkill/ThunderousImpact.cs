using System.Collections.Generic;

public class ThunderousImpact : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
       foreach(Entity target in targets)
        {
            target.ApplyEffect(new Stun());
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}