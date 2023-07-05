using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUntilTheEnd : GearSet
{  
    public override void FourPieces(Entity player) { _modifiers[Attribute.PhysicalDamages] = player.Stats[Attribute.PhysicalDamages].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.PhysicalDamages] = player.Stats[Attribute.PhysicalDamages].AddModifier(Buff6Pieces); }
}
