using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class Lost : State
    {
        public Lost(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("You were defeated");
            yield break;
        }
    }
}