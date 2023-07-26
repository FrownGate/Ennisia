using System.Collections.Generic;

public class WishingStar : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {

        foreach (Entity target in targets)
        {
            target.Heal(target.Stats[Attribute.HP].Value * Data.HealingAmount);
            target.Shield += target.Stats[Attribute.HP].Value * Data.ShieldAmount;
        }
        Cooldown = Data.MaxCooldown;

        return 0;
    }
}