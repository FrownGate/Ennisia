using System.Collections.Generic;

public class ShieldBump : Skill
{
    private void Awake()
    {
        FileName = "ShieldBump";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }   

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage)
    {
        //if targetHasDefBuff
        //float additionalDamage = damage / 2;
        //target.TakeDamage(additionalDamage);
        //return additionalDamage
        return 0;
    }
}