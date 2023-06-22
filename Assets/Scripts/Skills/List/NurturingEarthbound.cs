using System.Collections.Generic;

public class NurturingEarthbound : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float lostHealt = player.Stats[Item.AttributeStat.HP].Value - player.CurrentHp;
        HealingModifier = lostHealt * StatUpgrade1 * Level;
        return 0;
    }
}