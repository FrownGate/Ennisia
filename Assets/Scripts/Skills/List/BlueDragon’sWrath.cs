using System.Collections.Generic;

public class BlueDragonWraith : Skill
{
    private void Start()
    {
        FileName = "BlueDragonWraith";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.damageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.maxCooldown;
        return damage;
    }
}