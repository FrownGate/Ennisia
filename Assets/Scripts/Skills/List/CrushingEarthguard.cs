using System.Collections.Generic;

public class CrushingEarthguard : Skill
{
    //TODO -> Attacks all enemies, and gives a shield based off 40% of damage done.

    float totalDamage = 0;
    public override float Use(List<Entity> targets, Entity player, int turn) 
    {
        float damage = Data.DamageAmount;
       
        foreach (Entity target in targets)
        {
            target.TakeDamage(damage);
            totalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return totalDamage; 
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.Shield += (int)damage * (40 / 100);
    }



}
