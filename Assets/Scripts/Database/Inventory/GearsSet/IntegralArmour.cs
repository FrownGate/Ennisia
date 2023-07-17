using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class integralArmour : GearSet
{
    public override void FourPieces(Entity player) { _modifiers[Attribute.PhysicalDefense] = player.Stats[Attribute.MagicalDefense].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.PhysicalDefense] = player.Stats[Attribute.MagicalDamages].AddModifier(Buff6Pieces); }
}
