public class Earth : Element
{
    protected override void BuffElement(Entity _player)
    {
        _player.Stats[Attribute.MagicalDefense].AddModifier(Buff);
        _player.Stats[Attribute.PhysicalDefense].AddModifier(Buff);
    }

    private float Buff(float value)
    {
        return value * 0.1f;
    }
}