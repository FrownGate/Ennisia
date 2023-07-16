using System.Collections.Generic;

public class Revitalise : GearSet
{
    public override void FourPieces(Entity player) { _modifiers[Attribute.HP] = player.Stats[Attribute.MagicalDefense].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.HP] = player.Stats[Attribute.MagicalDamages].AddModifier(Buff6Pieces); }
}