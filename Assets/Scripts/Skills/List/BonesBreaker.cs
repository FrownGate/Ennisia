using System.Collections.Generic;

public class BonesBreaker : Skill
{
    private void Awake()
    {
        FileName = "BonesBreaker";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        //breakdef
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}