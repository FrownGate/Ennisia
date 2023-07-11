using System.Collections.Generic;
using System;

public class ArcaneTransference : BuffSkill
{
    private List<Effect> _debuffList;
    private int _debuff = 0;
    public override float Use(List<Entity> targets, Entity caster, int turn) 
    {

        //player.debuff -1
        foreach (Effect effect in caster.Effects)
        {
            if (!effect.HasAlteration) // Skill can't take in demonic mark, needs one more check
            {
                _debuffList[_debuff] = effect;
                _debuff++;
            }
        }
        int randomNumber = new Random().Next(0, _debuffList.Count);
        _debuffList[randomNumber].Cleanse(caster);

        //give debuff to enemy
        foreach (Entity target in targets)
        {
            target.ApplyEffect(_debuffList[randomNumber]);
        }
        return 0;
    }
}