public enum ElementType
{
    Fire, Water, Wind, Earth, Thunder
}

public class Element
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Element()
    {
        Name = GetType().Name;
    }

    public virtual void Init(Entity _player) { }
}