using System.Collections.Generic;
using System;

public class DemonicBite : DamageSkill
{
    private int[] _percentChances = { 100, 50 };
    private Effect[] _effects = { new DemonicMark(), new BreakDefense() };
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);

        for (int i = 0; i < _percentChances.Length; i++)
        {
            int randomNumber = new Random().Next(1, 100);

            if (_percentChances[i] >= randomNumber)
            {
                if(i == _percentChances.Length - 1)
                {
                    caster.AtkBarPercentage = 100;
                }
                targets[0].ApplyEffect(_effects[i]);
                //add demonic mark and defense break (if targets[0] is defense break, player.atkBarPercentage += 100;)
            }
        }

        Cooldown = Data.MaxCooldown;
        return damage;
    }
}