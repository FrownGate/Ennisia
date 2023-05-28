using System.Collections.Generic;

public class InAFlash : Skill
{
    private void Awake()
    {
        FileName = "InAFlash";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //give 50% ignoreDef to Enemy
        float damage = Data.damageAmount;
        targets[0].TakeDamage(damage);
        //Take it back
        Cooldown = Data.maxCooldown;
        return damage;
    }
}