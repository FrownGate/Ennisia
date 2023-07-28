using System.Collections.Generic;
using System;

public class ShieldBash : DamageSkill
{
    //TODO -> Inflicts damage based on 25% of his defense and has a 40% chance of Taunting. 
    private readonly int _percentChance = 40;

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster, Attribute.PhysicalDefense, Attribute.MagicalDefense);
            target.TakeDamage(damage * Data.DamageRatio);
            TotalDamage += damage;
            int randomNumber = new Random().Next(1, 100);
            if (_percentChance >= randomNumber)
            {
                //target.ApplyEffect(); add Taunt debuff
            }
        }
        
        return TotalDamage; 
    }
}