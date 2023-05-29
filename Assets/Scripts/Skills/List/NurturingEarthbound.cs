using System.Collections.Generic;

public class NurturingEarthbound : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float lostHealt = player.MaxHp - player.CurrentHp;
        player.CurrentHp += lostHealt * Data.HealingAmount / 100;
        return 0;
    }
}