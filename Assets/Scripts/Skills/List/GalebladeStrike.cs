using System.Collections.Generic;

public class GalebladeStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //buff Atk
        float damage = 0;
        targets[0].TakeDamage(damage);
        return damage;
    }
}