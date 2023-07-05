using System.Collections.Generic;

public class InAFlash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.DefIgnored] = targets[0].Stats[Attribute.DefIgnored].AddModifier(Add50); //targets[0] or caster ?
        float damage = Data.DamageRatio;
        targets[0].TakeDamage(damage);
        TakeOffStats(caster);
        return damage;
    }

    float Add50(float value) => value + 50; //penDef
}