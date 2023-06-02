using System.Collections;
using UnityEngine;

public class Won : State
{

    public Won(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        foreach (var skill in BattleSystem.Player.Skills)
        {
            skill.TakeOffStats(BattleSystem.Enemies, BattleSystem.Player, 0); // constant passive at battle end
        }
        
        BattleSystem.dialogueText.text = "YOU WON THE FIGHT";
        BattleSystem._wonPopUp.SetActive(true);
        yield break;
    }
}