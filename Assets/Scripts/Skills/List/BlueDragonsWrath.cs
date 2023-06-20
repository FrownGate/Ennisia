using System.Collections.Generic;

public class BlueDragonsWrath : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        //Boost 50% attack for 3 turns
        new BuffEffect(3, entity =>
        {
            entity.AlterateStat(Item.AttributeStat.Attack, value => value * 1.5f, 1);
        } );

        return damage;
    }
}