using System.Collections.Generic;

public class CrushingEarthguard : DamageSkill
{
    //TODO -> Attacks all enemies, and gives a shield based off 40% of damage done.

    private float _totalDamage = 0;

    public override float Use(List<Entity> targets, Entity caster, int turn) 
    {
        float damage = Data.DamageAmount;
       
        foreach (Entity target in targets)
        {
            target.TakeDamage(damage);
            _totalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return _totalDamage; 
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.Shield += (int)damage * (40 / 100);
    }
}