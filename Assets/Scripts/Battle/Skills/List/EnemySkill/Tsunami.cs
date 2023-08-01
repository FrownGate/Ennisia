using System.Collections.Generic;
using Unity.VisualScripting;

public class Tsunami : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            target.AtkBar -= 30;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }       
}