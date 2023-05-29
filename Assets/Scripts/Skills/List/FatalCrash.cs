using System.Collections.Generic;

public class FatalCrash : Skill
{
    private void Start()
    {
        FileName = "FatalCrash";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount * ((targets[0].CurrentHp + 100) / targets[0].MaxHp); //HUGO TO BALANCE -> make excel
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.CurrentHp += 80/100 * damage;
    }
}