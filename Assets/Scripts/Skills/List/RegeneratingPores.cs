using System.Collections.Generic;

public class RegeneratingSpores : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        for (int i = 1; i < targets.Count; i++) targets[i].CurrentHp += (targets[i].Stats[Attribute.HP].Value * 0.6f);
        return 0;
    }

    // add the clense all debuffs
}