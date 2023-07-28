using System.Collections.Generic;

public class NurturingEarthbound : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        float _lostHealt = caster.Stats[Attribute.HP].Value - caster.CurrentHp;
        float _healingRatio = (Data.HealingAmount + (StatUpgrade1 * Level)) / 100;
        caster.Heal(_lostHealt * _healingRatio);
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}