using System.Collections.Generic;

public class FastShield : Skill
{
    //TODO -> After an enemy attack, gives a shield, the shield strength is equivalent to 20% of the damage received  

    public override void UseIfAttacked(List<Entity> targets, Entity player, int turn, float damageTaken)
    {
        float shieldAmount = damageTaken * Data.ShieldAmount / 100;
        player.Shield += shieldAmount;
    }

}
