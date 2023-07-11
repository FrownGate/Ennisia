using System.Collections.Generic;

public class BreezeOfVitality : ProtectionSkill
{
    private readonly float _increaseHealPerc = 10f;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float addHeal = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].CurrentHp > 0)
            {
                addHeal += _increaseHealPerc;
            }
        }

        float heal = caster.Stats[Attribute.HP].Value * (Data.HealingAmount + addHeal) / 100;
        caster.CurrentHp += heal;

        return 0;
    }
}