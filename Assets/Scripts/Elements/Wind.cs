public class Wind : Element
{
    protected override void BuffElement(Entity _player)
    {
        _player.Stats[Attribute.CritDmg].AddModifier(Buff);
    }

    private float Buff(float value)
    {
        return value + 20;
    }
}