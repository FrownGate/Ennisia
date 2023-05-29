using System.Collections.Generic;

public class InfernalResilience : Skill
{
    public override float Use(List<Entity> target, Entity player, int turn)
    {
        float missingHealth = player.MaxHp - player.CurrentHp;
        float shield = missingHealth * Data.ShieldAmount / 100;
        //give shield for 3 turn for shieldamount
        return 0;
    }
}