using System.Collections.Generic;

public class TidalExecution : Skill
{
    private void Awake()
    {
        FileName = "TidalExecution";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float percHPRemaining = targets[0].CurrentHp / targets[0].MaxHp;

        if (percHPRemaining <= 0.05f)
        {
            //TODO -> Execute
            targets[0].TakeDamage(targets[0].MaxHp);
        }

        float missingHealth = targets[0].MaxHp - targets[0].CurrentHp;
        float damage = Data.DamageAmount * missingHealth;
        targets[0].TakeDamage(damage);

        return damage;
    }
}