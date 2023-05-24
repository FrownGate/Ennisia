using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class SelectSpell : PlayerTurn
    {
        protected int _spellNumber;
        public SelectSpell(BattleSystem battleSystem) : base(battleSystem)
        {
            _spellNumber = battleSystem.ButtonId;
        }

        public override IEnumerator Start()
        {
            Debug.Log(_spellNumber);
            BattleSystem.dialogueText.text = "Select a spell";
            BattleSystem.SetState(new SelectTarget(BattleSystem));
            yield break;
        }
        
        

        
        
    }
}