using System.Collections.Generic;

public class InfernalResilience : Skill
{
    public override float Use(List<Entity> target, Entity player, int turn)
    {
        float missingHealth = player.MaxHp - player.CurrentHp;
        ShieldModifier = missingHealth * StatUpgrade1 * Level;
        //give shield for 3 turn for shieldamount
        return 0;
    }
}