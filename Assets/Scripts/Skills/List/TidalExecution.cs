using System.Collections.Generic;

public class TidalExecution : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float percHPRemaining = targets[0].CurrentHp / targets[0].Stats[Item.AttributeStat.HP].Value;

        if (percHPRemaining <= 0.05f)
        {
            //TODO -> Execute
            targets[0].TakeDamage(targets[0].Stats[Item.AttributeStat.HP].Value);
        }

        float missingHealth = targets[0].Stats[Item.AttributeStat.HP].Value - targets[0].CurrentHp;
        float damage = Data.DamageAmount * missingHealth;
        targets[0].TakeDamage(damage);

        return damage;
    }
}