using System.Collections.Generic;

public class RefreshingTideguard : ProtectionSkill
{
    //TODO -> Gives a shield to the player, scales with max Hp, for 2 turns.


    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        player.Shield += 10;
        return 0;
    }

}
