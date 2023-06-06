using System.Collections.Generic;

public class NurturingEarthbound : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float lostHealt = player.MaxHp - player.CurrentHp;
        HealingModifier = lostHealt * StatUpgrade1 * Level;
        return 0;
    }
}