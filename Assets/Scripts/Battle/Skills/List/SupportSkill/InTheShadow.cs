using System.Collections.Generic;
using System;
public class InTheShadow : DamageSkill
{
    private int _percentChance = 50;
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored += Data.IgnoreDef;
        }
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored -= Data.IgnoreDef;
            if (target.Effects.Count > 0)
            {
                float _randomNumber = new Random().Next(1, 100);

                if (_percentChance >= _randomNumber)
                {
                    player.AtkBarPercentage = 100;
                }
                break;
            }
        }

    }
}