using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class Won : State
    {
        public Won(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("You won");
           yield break;
        }
    }
}