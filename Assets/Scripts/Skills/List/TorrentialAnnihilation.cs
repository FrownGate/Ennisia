using System.Collections.Generic;

public class TorrentialAnnihilation : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (caster.Stats[Attribute.PhysicalDamages].Value > targets[i].Stats[Attribute.PhysicalDamages].Value)
            {
                //TODO -> cleanse target's buff
            }
        }

        return 0;
    }

    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        for (int i = 0; i < targets.Count; i++) targets[i].TakeDamage(damage * 0.25f);
        return 0;
    }
}