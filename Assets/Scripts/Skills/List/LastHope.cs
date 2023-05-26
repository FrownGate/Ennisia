using System.Collections.Generic;

public class LastHope : Skill
{
    private bool _isUsed;

    private void Awake()
    {
        _isUsed = false;
        FileName = "LastHope";
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        if(player.CurrentHp < player.MaxHp * 0.2f & _isUsed)
        {
            _isUsed = true;
            HealingModifier = player.MaxHp * 0.3f;
        }
    }
}