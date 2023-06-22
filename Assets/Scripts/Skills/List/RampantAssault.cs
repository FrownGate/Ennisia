using System.Collections.Generic;

public class RampantAssault : DamageSkill
{
    public float targetMaxHpBaseRatio;

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        DamageModifier = targets[0].Stats[Item.AttributeStat.HP].Value * (targetMaxHpBaseRatio * StatUpgrade1 * Level);
        float percOfAddDamage = StatUpgrade2 * turn;

        percOfAddDamage = percOfAddDamage > (percOfAddDamage*5) ? (percOfAddDamage*5) : percOfAddDamage;

        float damagePerTurn = DamageModifier * percOfAddDamage;
        DamageModifier += damagePerTurn;
        return  0;
    }
}