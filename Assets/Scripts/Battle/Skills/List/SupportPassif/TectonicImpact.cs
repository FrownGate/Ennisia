using System.Collections.Generic;

public class TectonicImpact : DamageSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(damage * (Data.BuffAmount / 100));
        }
    }
}