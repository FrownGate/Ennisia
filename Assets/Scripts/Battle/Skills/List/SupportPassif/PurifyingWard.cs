using System.Collections.Generic;
using System;

public class PurifyingWard : PassiveSkill
{
    private List<Effect> _debuffList;
    private int _debuffCount;

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage, List<Entity> allies)
    {
        foreach (Effect effect in caster.Effects)
        {
            if (!effect.HasAlteration) 
            {
                _debuffList[_debuffCount] = effect;
                _debuffCount++;
            }
        }

        int randomNumber = new Random().Next(0, caster.Effects.Count); //Used ?
        _debuffList[_debuffCount].RemoveEffect();
    }
}