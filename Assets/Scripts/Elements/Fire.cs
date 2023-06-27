public class Fire : Element
{
    protected override void BuffElement(Entity _player)
    {
        _player.Stats[Item.AttributeStat.HP].AddModifier(Buff);
    }

    private float Buff(float value)
    {
        return value * 0.15f;
    }
}