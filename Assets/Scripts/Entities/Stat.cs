using System;
using System.Collections.Generic;

public class ModifierID { }

public class Stat<T>
{
    private class Modifier
    {
        public ModifierID Id;
        public Func<T, T> Func;
        public int Layer;
    }

    private readonly T _initialValue;
    private readonly List<Modifier> _modifiers;

    public event Action<ModifierID> OnModifierAdded;
    public event Action<ModifierID> OnModifierRemoved;
    public event Action OnAllModifiersRemoved;

    public T Value
    {
        get
        {
            T tmpValue = _initialValue;

            foreach (Modifier modifier in _modifiers)
            {
                tmpValue = modifier.Func(tmpValue);
            }

            return tmpValue;
        }
    }

    public Stat(T value)
    {
        _initialValue = value;
        _modifiers = new();
    }

    public ModifierID AddModifier(Func<T, T> func, int layer = 1)
    {
        ModifierID id = new();

        Modifier modifier = new()
        {
            Id = id,
            Func = func,
            Layer = layer
        };

        if (_modifiers.Count > 0 && _modifiers[_modifiers.Count - 1].Layer > layer)
        {
            int index = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                index = i;
                if (layer < _modifiers[i].Layer) break;
            }

            _modifiers.Insert(index, modifier);
        }
        else
        {
            _modifiers.Add(modifier);
        }

        OnModifierAdded?.Invoke(id);
        return id;
    }

    public bool RemoveModifier(ModifierID id)
    {
        Modifier modifier = _modifiers.Find(modifier => modifier.Id == id);
        if (modifier == null) return false;
        _modifiers.Remove(modifier);
        OnModifierRemoved?.Invoke(id);
        return true;
    }

    public void RemoveAllModifiers()
    {
        List<Modifier> modifiers = new();

        foreach (var modifier in _modifiers)
        {
            if (modifier.Layer == 0) modifiers.Add(modifier);
        }

        _modifiers.Clear();
        _modifiers.AddRange(modifiers);
        OnAllModifiersRemoved?.Invoke();
    }
}