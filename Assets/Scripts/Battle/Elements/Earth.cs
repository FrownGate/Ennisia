public class Earth : Element
{
    public override void Init(Entity _player)
    {
        _player.Stats[Attribute.MagicalDefense].AddModifier(Buff);
        _player.Stats[Attribute.PhysicalDefense].AddModifier(Buff);
    }

    private float Buff(float value) => value * 0.1f;
}