using System.Collections.Generic;

public class NurturingEarthbound : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float lostHealt = caster.Stats[Attribute.HP].Value - caster.CurrentHp;
        HealingModifier = lostHealt * StatUpgrade1 * Level;
        return 0;
    }
}