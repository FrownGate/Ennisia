using System.Collections.Generic;

public class TectonicImpact : DamageSkill
{
    private float _damage = 0;
    private float _totalDamage;
    //TODO -> When the player attacks a single enemy, deal 15% of damage done to all other enemies.

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }

        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].TakeDamage(_damage * (15 / 100));
            TotalDamage += _damage * (15 / 100);
        }

        return TotalDamage;
    }
}