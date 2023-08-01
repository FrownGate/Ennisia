using System.Collections.Generic;

public class CrushingEarthguard : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies) 
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target,caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return TotalDamage; 
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage,
        List<Entity> allies)
    {
        caster.Shield += damage * Data.ShieldAmount;
    }
}