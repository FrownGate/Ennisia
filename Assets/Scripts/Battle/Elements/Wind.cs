public class Wind : Element
{
    public override void Init(Entity _player)
    {
        _player.Stats[Attribute.CritDmg].AddModifier(Buff);
    }

    private float Buff(float value) => value + 20;
}