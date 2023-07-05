using System.Collections.Generic;

public class BitingBarrage : DamageSkill
{
    private float _damage;
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            _damage = DamageCalculation(target, caster);
            target.TakeDamage(_damage);
        }
        Cooldown = Data.MaxCooldown;
        return _damage;
    }
}