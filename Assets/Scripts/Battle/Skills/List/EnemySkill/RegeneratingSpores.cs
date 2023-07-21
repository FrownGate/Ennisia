using System.Collections.Generic;

public class RegeneratingSpores : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            target.Cleanse();
            target.Heal(target.Stats[Attribute.HP].Value*(Data.HealingAmount/100));
        }
        return 0;
    }
}