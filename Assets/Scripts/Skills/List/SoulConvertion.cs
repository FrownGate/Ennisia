using System.Collections.Generic;

public class SoulConvertion : Skill
{
    public float DmgBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        bool weaponDmgType = !player.WeaponSO.isMagic;
        float weaponDmgBuff = player.WeaponSO.statValue * (DmgBaseRatio + StatUpgrade1 * Level);
        player.WeaponSO.isMagic = weaponDmgType;
        player.WeaponSO.statValue += weaponDmgBuff;
    }
}
