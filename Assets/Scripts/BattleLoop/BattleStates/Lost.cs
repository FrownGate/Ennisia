using System.Collections;
using UnityEngine;

public class Lost : State
{
    public Lost(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        Debug.Log("You were defeated");
        yield break;
    }
}