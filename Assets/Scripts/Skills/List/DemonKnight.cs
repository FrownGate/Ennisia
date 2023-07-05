using System.Collections.Generic;

public class DemonKnight : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Effect effect in targets[0].Effects)
        {
            if (effect.GetType() == typeof(DemonicMark))
            {
                targets[0].ApplyEffect(new DemonicMark());
                targets[0].ApplyEffect(new DemonicMark());
            }
        }
    }
}