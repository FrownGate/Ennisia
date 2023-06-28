using System.Collections.Generic;

public class Bonk : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //damage calculation
        int damage = 10000;
        targets[0].TakeDamage(damage);
        return damage;
    }
}