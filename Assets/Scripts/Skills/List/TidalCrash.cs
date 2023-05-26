using System.Collections.Generic;

public class TidalCrash : Skill
{
    private void Awake()
    {
        FileName = "TidalCrash";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float TotalDamage = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            float damage = Data.damageAmount;
            targets[i].TakeDamage(damage);
            TotalDamage += damage;
        }

        Cooldown = Data.maxCooldown;
        return TotalDamage;
    }
}