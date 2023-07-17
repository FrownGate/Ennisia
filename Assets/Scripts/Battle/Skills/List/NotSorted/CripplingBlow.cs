using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : DamageSkill
{
    //support skill

    // TO DO -> add defense debuff for x turns
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //this ratio will be added with the Data.DamageRatio in the damage calculation
        RatioModifier = (StatUpgrade1 * Level * caster.Stats[Attribute.Attack].Value * caster.Stats[Attribute.PhysicalDamages].Value);
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            int debuffLuck = Random.Range(0, 100);
            if (debuffLuck <= 80)
            {
                target.ApplyEffect(new BreakDefense());
            }
        }
        return TotalDamage;
    }
}