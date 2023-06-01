using System.Collections.Generic;

public class SoulConvertion : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        bool weaponDmgType = !player.WeaponSO.isMagic;
        float weaponDmgBuff = player.WeaponSO.statValue * 0.2f;
        player.WeaponSO.isMagic = weaponDmgType;
        player.WeaponSO.statValue += weaponDmgBuff;
    }
}
