using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TerraShockwave: DamageSkill
{
    [ShowInInspector] private float _stunPerc = 0.5f;
    [ShowInInspector] private float _increaseCDPerc = 0.75f;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            RatioModifier = StatUpgrade1 * Level;
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;

            float _luck = Random.Range(0, 100);
            if (_luck <= _stunPerc)
            {
                target.ApplyEffect(new Stun(1));
            }
            float _luck2 = Random.Range(0, 100);
            if (_luck2 <= _increaseCDPerc)
            {
                //TO DO -> increase target third skill cd by 2
            }
        }
        Cooldown = Data.MaxCooldown;
        
        return TotalDamage;
    }
}