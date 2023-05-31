using System.Collections.Generic;

public class HolyPrayer : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //attacksBuff

        player.CurrentHp += player.MaxHp * 10 / 100;
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}