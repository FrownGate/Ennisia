using System.Collections.Generic;


public class GearSet
{
    protected Dictionary<Attribute, ModifierID> _modifiers;
    public int Value4Pieces { get; protected set; }
    public int Value6Pieces { get; protected set; }
    public virtual void FourPieces(Entity player) { }
    public virtual void SixPieces(Entity player) { }
    public virtual float Buff4Pieces(float value) => value * (1 + Value4Pieces);
    public virtual float Buff6Pieces(float value) => value * (1 + Value6Pieces);
}
