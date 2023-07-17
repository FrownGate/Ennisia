public class Water : Element
{
    public override void Init(Entity _player)
    {
        _player.Stats[Attribute.CritRate].AddModifier(Buff);
    }

    private float Buff(float value) => value + 10;
}