using System.Collections.Generic;

public class FatalCrash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = Data.DamageAmount * ((targets[0].CurrentHp + 100) / targets[0].Stats[Attribute.HP].Value); //HUGO TO BALANCE -> make excel
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.CurrentHp += 80/100 * damage;
    }
}