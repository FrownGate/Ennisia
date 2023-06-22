using System.Collections.Generic;
using UnityEngine;

public class BlueDragonsWrath : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;

        return damage;
    }
}