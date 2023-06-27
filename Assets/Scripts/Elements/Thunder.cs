public class Thunder : Element
{
    protected override void BuffElement(Entity _player)
    {
        _player.Stats[Item.AttributeStat.Attack].AddModifier(Buff);
    }

    private float Buff(float value)
    {
        return value * 0.1f;
    }
}