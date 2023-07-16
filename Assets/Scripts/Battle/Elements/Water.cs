public class Water : Element
{
    protected override void BuffElement(Entity _player)
    {
        _player.Stats[Attribute.CritRate].AddModifier(Buff);
    }

    private float Buff(float value)
    {
        return value + 10;
    }
}