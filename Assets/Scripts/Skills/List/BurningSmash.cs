using System.Collections.Generic;
using System;

public class BurningSmash : DamageSkill
{
    int percentChance = 20;
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        Random rand = new Random();
        int randomNumber = rand.Next(1, 100);
        if (percentChance >= randomNumber)
        {
            //add defense break
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}