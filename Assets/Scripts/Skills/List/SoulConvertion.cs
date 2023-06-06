using System.Collections.Generic;

public class SoulConvertion : Skill
{
    public float DmgBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        bool weaponDmgType = !player.WeaponSO.IsMagic;
        float weaponDmgBuff = player.WeaponSO.StatValue * (DmgBaseRatio + StatUpgrade1 * Level);
        player.WeaponSO.IsMagic = weaponDmgType;
        player.WeaponSO.StatValue += weaponDmgBuff;
    }
}
