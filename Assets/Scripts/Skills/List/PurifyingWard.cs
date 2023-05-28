using System.Collections.Generic;

public class PurifyingWard : Skill
{
    private void Awake()
    {
        FileName = "PurifyingWard";
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        // to do : cleanse one debuff every turn
    }
}