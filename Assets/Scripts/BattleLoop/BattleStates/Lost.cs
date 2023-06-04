using System.Collections;
using UnityEngine;

public class Lost : State
{
    public Lost(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        foreach (var skill in BattleSystem.Player.Skills)
        {
            skill.TakeOffStats(BattleSystem.Enemies, BattleSystem.Player, 0); // constant passive at battle end
        }

        BattleSystem.DialogueText.text = "YOU LOST THE FIGHT";
        BattleSystem.LostPopUp.SetActive(true);
        yield break;
    }
}