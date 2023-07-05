using System.Collections.Generic;

public class LuckyPull : GearSet
{
    public override void FourPieces(Entity player) { _modifiers[Attribute.CritRate] = player.Stats[Attribute.MagicalDefense].AddModifier(Buff4Pieces); }
    public override void SixPieces(Entity player) { _modifiers[Attribute.CritRate] = player.Stats[Attribute.MagicalDamages].AddModifier(Buff6Pieces); }
}