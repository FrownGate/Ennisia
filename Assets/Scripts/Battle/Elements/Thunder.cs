public class Thunder : Element
{
    public override void Init(Entity _player)
    {
        _player.Stats[Attribute.Attack].AddModifier(Buff);
    }

    private float Buff(float value) => value * 0.1f;
}