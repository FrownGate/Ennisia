using System.Collections.Generic;

public class Herald : GearSet
{
    public override void FourPieces(Entity player) { _modifiers[Attribute.Speed] = player.Stats[Attribute.MagicalDefense].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.Speed] = player.Stats[Attribute.MagicalDamages].AddModifier(Buff6Pieces); }
}