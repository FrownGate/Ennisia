using System.Collections.Generic;
using static Stat<float>;

public class InAFlash : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        ModifierID id  = targets[0].Stats[Item.AttributeStat.DefIgnoref].AddModifier(Add50);
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        targets[0].Stats[Item.AttributeStat.DefIgnoref].RemoveModifier(id);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    float Add50(float input) //penDef
    {
        return input + 50;
    }
}