using System.Collections.Generic;
<<<<<<< HEAD

public class ShieldBash : Skill
{
//TODO -> Inflicts damage based on 25% of his defense and has a 40% chance of Taunting. 
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
=======
using System;

public class ShieldBash : DamageSkill
{
    //TODO -> Inflicts damage based on 25% of his defense and has a 40% chance of Taunting.
    private readonly int _percentChance = 40;
    public override float Use(List<Entity> targets, Entity caster, int turn)
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
>>>>>>> feature/BossSkill
}
