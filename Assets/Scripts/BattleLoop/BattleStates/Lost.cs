using System.Collections;

public class Lost : State
{
    public Lost(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.LostPopUp.SetActive(true);
        BattleSystem.BattleEnded(false);
        yield break;
    }
}