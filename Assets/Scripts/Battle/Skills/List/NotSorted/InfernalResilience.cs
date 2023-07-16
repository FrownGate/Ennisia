using System.Collections.Generic;

public class InfernalResilience : ProtectionSkill
{
    public override float Use(List<Entity> target, Entity caster, int turn)
    {
        float missingHealth = caster.Stats[Attribute.HP].Value - caster.CurrentHp;
        ShieldModifier = missingHealth * StatUpgrade1 * Level;
        caster.Shield += ShieldModifier;
        return 0;
    }
}