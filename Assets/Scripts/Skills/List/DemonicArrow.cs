using System.Collections.Generic;
using System;

public class DemonicArrow : DamageSkill
{
    int randomNumber;
    int percentChance = 40;
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        Random rand = new Random();
        randomNumber = rand.Next(1, 100);
        if (percentChance >= randomNumber)
        {
            //add defense debuff
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}   