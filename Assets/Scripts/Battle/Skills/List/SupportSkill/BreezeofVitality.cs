using System.Collections.Generic;

public class BreezeOfVitality : ProtectionSkill
{

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float addHeal = 0;

        foreach (Entity target in targets)
        {
            addHeal += Data.BuffAmount;
        }
        caster.Heal(caster.Stats[Attribute.HP].Value * ((Data.HealingAmount + addHeal) / 100));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}