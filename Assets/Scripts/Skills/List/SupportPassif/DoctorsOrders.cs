using System.Collections.Generic;

public class DoctorsOrders : PassiveSkill
{
    public override void UseIfAttacked(List<Entity> targets, Entity player, Entity caster, int turn, float damageTaken)
    {
        if (player.CurrentHp <= 0)
        {
            _modifiers[Attribute.HP] = caster.Stats[Attribute.HP].AddModifier(AddHPBuff);
        }
    }
    float AddHPBuff(float value) => 1;
}
