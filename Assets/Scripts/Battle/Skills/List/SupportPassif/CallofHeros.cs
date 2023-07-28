using System.Collections.Generic;

public class CallofHeros : PassiveSkill
{
    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken,
        List<Entity> allies)
    {
        if (player.CurrentHp <= (player.Stats[Attribute.HP].Value * 0.40f))
        {
            player.Shield += (caster.Stats[Attribute.HP].Value * Data.ShieldAmount);
            player.ApplyEffect(new AttackBuff(2));
        }
    }
}
