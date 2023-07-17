using System.Collections;

public abstract class State
{
    protected BattleSystem BattleSystem;

    public State(BattleSystem battleSystem)
    {
        BattleSystem = battleSystem;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Attack()
    {
        yield break;
    }

    public virtual IEnumerator Heal()
    {
        yield break;
    }

    public virtual IEnumerator UseSpell()
    {
        yield break;
    }
}