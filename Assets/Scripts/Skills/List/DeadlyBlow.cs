using System.Collections.Generic;

public class DeadlyBlow : Skill
{
    private void Start()
    {
        FileName = "DeadlyBlow";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.damageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.maxCooldown;
        return damage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        if (targets[0].IsDead) Cooldown = 0;
    }
}