using System.Collections.Generic;

public class Bonk : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //damage calculation
        int damage = 0;
        targets[0].TakeDamage(damage);
        player.ApplyEffect(new ATKBUFF(3,player)); //TODO -> Remove when test is done
        return damage;
    }
}