using System.Collections.Generic;

public class CallofHeros : PassiveSkill
{
    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken)
    {
        if (player.CurrentHp <= (player.Stats[Attribute.HP].Value * 0.40f))
        {
            player.Shield += (int)(caster.Stats[Attribute.HP].Value * 0.70f);
            player.ApplyEffect(new AttackBuff(2));
        }
    }
}
