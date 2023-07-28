using System.Collections.Generic;

public class RampantAssault : DamageSkill
{
    private float _targetMaxHpBaseRatio;

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            float damage = target.Stats[Attribute.HP].Value * ((Data.DamageRatio/100) + StatUpgrade1 * Level);
            damage = DamageCalculation(target, caster);
            float percOfAddDamage = StatUpgrade2 * turn;
            percOfAddDamage = percOfAddDamage > (percOfAddDamage * 5) ? (percOfAddDamage * 5) : percOfAddDamage;
            
            damage *= percOfAddDamage;
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
       
        return TotalDamage;
    }
}