using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : GearSet
{
    public override void FourPieces(Entity player) { _modifiers[Attribute.MagicalDefense] = player.Stats[Attribute.MagicalDefense].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.MagicalDamages] = player.Stats[Attribute.MagicalDamages].AddModifier(Buff6Pieces); }
}
