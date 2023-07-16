public class Fire : Element
{
    public override void Init(Entity _player)
    {
        _player.Stats[Attribute.HP].AddModifier(Buff);
    }

    private float Buff(float value) => value * 0.15f;
}