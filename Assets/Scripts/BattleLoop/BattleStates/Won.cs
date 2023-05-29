using System.Collections;
using UnityEngine;

public class Won : State
{
    public Won(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        foreach (var skill in BattleSystem.Allies[0].Skills)
        {
            skill.TakeOffStats(BattleSystem.Enemies, BattleSystem.Allies[0], 0); // constant passive at battle end
        }
        Debug.Log("You won");
        yield break;
    }
}