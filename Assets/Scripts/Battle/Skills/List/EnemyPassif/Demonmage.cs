using System.Collections.Generic;

public class DemonMage : PassiveSkill
{
    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        float dmgOnHeal = 0;
        if (caster.Healed)
        {
            dmgOnHeal = caster.AmountHealed;
        }
        return dmgOnHeal;
    }
}