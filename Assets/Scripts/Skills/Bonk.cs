using System.Collections.Generic;

public class Bonk : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //damage calculation
        int damage = 0;
        targets[0].TakeDamage(damage);
        return damage;
    }
}