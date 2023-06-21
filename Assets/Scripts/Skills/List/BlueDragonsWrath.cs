using System.Collections.Generic;

public class BlueDragonsWrath : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);

        return damage;
    }
}