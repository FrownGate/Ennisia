﻿using System.Collections;

public class Won : State
{
    public Won(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.WonPopUp.SetActive(true);
        BattleSystem.EndWave(true);
        yield break;
    }
}