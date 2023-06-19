using System.Collections.Generic;

public class CrushingEarthguard : Skill
{
//TODO -> Attacks all enemies, and gives a shield based off 40% of damage done.
    

    public override float Use(List<Entity> targets, Entity player, int turn) 
    {
        float damage = Data.DamageAmount;
        float totalDamage = 0;
        foreach (Entity target in targets)
        {
            target.TakeDamage(damage);
            totalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return totalDamage; 
    }


    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) 
    {
        //gain 40% of damage as shield
    }


}
