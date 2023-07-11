using System.Collections.Generic;

public class GoblinChief : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Effect effect in caster.Effects)
        {
            if (effect.GetType() == typeof(Stun))
            {
                targets[0].DefIgnored += 50;
            }
        }
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        targets[0].DefIgnored -= 50;
    }
}