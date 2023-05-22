using System.Collections;

namespace BattleLoop.BattleStates
{
    public class SelectSpell : PlayerTurn
    {
        public SelectSpell(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            BattleSystem.dialogueText.text = "Select a spell";
            yield break;
            
        }

        public override IEnumerator UseSpell()
        {
            
            
            return base.UseSpell();
        }
        
    }
}