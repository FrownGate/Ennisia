using System.Collections.Generic;
using System;
public class PurifyingWard : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        int randomNumber = new Random().Next(0, caster.Effects.Count);

        //take off 1 debugff
    }
}