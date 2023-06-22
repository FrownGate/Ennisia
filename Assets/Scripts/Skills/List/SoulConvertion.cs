using System.Collections.Generic;

public class SoulConvertion : PassiveSkill
{
    public float DmgBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        bool weaponDmgType = !player.Weapon.IsMagic;
        float weaponDmgBuff = player.Weapon.StatValue * (DmgBaseRatio + StatUpgrade1 * Level);
        player.Weapon.IsMagic = weaponDmgType;
        player.Weapon.StatValue += weaponDmgBuff;
    }
}
