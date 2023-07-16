using System.Collections.Generic;

public class RampantAssault : DamageSkill
{
    private float _targetMaxHpBaseRatio;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        DamageModifier = targets[0].Stats[Attribute.HP].Value * (_targetMaxHpBaseRatio * StatUpgrade1 * Level);
        float percOfAddDamage = StatUpgrade2 * turn;

        percOfAddDamage = percOfAddDamage > (percOfAddDamage*5) ? (percOfAddDamage*5) : percOfAddDamage;

        float damagePerTurn = DamageModifier * percOfAddDamage;
        DamageModifier += damagePerTurn;
        return  0;
    }
}