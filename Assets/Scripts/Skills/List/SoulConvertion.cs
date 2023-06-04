using System.Collections.Generic;

public class SoulConvertion : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        bool weaponDmgType = !player.WeaponSO.IsMagic;
        float weaponDmgBuff = player.WeaponSO.StatValue * 0.2f;
        player.WeaponSO.IsMagic = weaponDmgType;
        player.WeaponSO.StatValue += weaponDmgBuff;
    }
}
