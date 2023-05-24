using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class PlayerTurn : State
    {
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            BattleSystem.dialogueText.text = "Your turn";
            yield return new WaitForSeconds(1.0f);
        }


        
    }
}