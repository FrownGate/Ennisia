using System.Collections.Generic;
using System;

public class DemonicMarking : DamageSkill
{
    int[] percentChances = {100, 50, 50};
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);
        for (int i = 0; i < percentChances.Length; i++)
        {
            Random rand = new Random();
            int randomNumber = rand.Next(1, 100);

            if (percentChances[i] >= randomNumber)
            {
                //add defense debuff, Crit Rate Buff, Crit Dmg Buff
            }
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}