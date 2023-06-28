using System.Collections.Generic;
using System;

public class DemonicSlash : DamageSkill
{
    int randomNumber;
    int percentChance = 10;
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);
        Random rand = new Random();
        randomNumber = rand.Next(1, 100);
        if (percentChance >= randomNumber)
        {
            //add demonic mark
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }

}