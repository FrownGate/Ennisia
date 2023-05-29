using System.Collections.Generic;

public class BloodContract : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
       float TotalDamage = 0;

       for (int i = 0; i < targets.Count; i++)
        {
            float damage = Data.DamageAmount;
            targets[i].TakeDamage(damage);
            TotalDamage += damage;
        }

        return TotalDamage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.CurrentHp += damage * 30 / 100;
    }
}