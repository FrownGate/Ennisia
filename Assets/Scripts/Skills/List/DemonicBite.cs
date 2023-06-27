using System.Collections.Generic;
using System;

public class DemonicBite : DamageSkill
{
    int[] percentChances = { 100, 50 };
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        for (int i = 0; i < percentChances.Length; i++)
        {
            Random rand = new Random();
            int randomNumber = rand.Next(1, 100);

            if (percentChances[i] >= randomNumber)
            {
                //add demonic mark and defense break (if targets[0] is defense break, player.atkBarPercentage += 100;)

            }
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}