using System.Collections.Generic;

public class DemonKnight : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            foreach (Effect effect in target.Effects)
            {
                if (effect.GetType() == typeof(DemonicMark))
                {
                    target.ApplyEffect(new DemonicMark());
                    target.ApplyEffect(new DemonicMark());
                    break;
                }
            }
        }
        
    }
}