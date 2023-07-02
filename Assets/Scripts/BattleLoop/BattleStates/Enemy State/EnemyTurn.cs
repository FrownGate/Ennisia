using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.ResetSelectedEnemies();
        BattleSystem.SetSkillButtonsActive(false);
        BattleSystem.DialogueText.text = "Enemy " + BattleSystem.EnemyPlayingID + "turn";
        BattleSystem.Enemies[BattleSystem.EnemyPlayingID].AtkBar = 0;
        //BattleSystem.Player.TakeDamage(BattleSystem.Enemies[0].Attack);
        //BattleSystem.Player.ApplyEffect(new SILENCE(4,BattleSystem.Player));

        yield return new WaitForSeconds(1f);

        BattleSystem.SetState(new CheckTurn(BattleSystem));
    }

    public override IEnumerator Attack()
    {

        bool CanProtect = false;
        bool CanBuff = false;
        bool TeammateLow = false;
        List<Skill> list = BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills;
        list.Reverse();

        foreach (Skill skill in list)
        {
            if (skill.GetType() == typeof(ProtectionSkill) && skill.Cooldown == 0)
            {
                CanProtect = true;
                break;
            }
        }
        foreach (Skill skill in list)
        {
            if (skill.GetType() == typeof(BuffSkill) && skill.Cooldown == 0)
            {
                CanBuff = true;
                break;
            }
        }
        foreach (Entity enemy in BattleSystem.Enemies)
        {
            if (enemy.CurrentHp < enemy.Stats[Item.AttributeStat.HP].Value * 20 / 100)
            {
                TeammateLow = true;
                break;
            }
        }

        foreach (Skill skill in list)
        {
            if (skill.Cooldown == 0 && skill.GetType() != typeof(PassiveSkill))
            {
                if ((BattleSystem.Enemies[BattleSystem.EnemyPlayingID].CurrentHp < BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Stats[Item.AttributeStat.HP].Value * 20 / 100 || TeammateLow) && CanProtect)
                { //if i'm low and i can protect myself // modify to check all the team hp

                    if (skill.GetType() == typeof(ProtectionSkill)) //i check if my skill alow me to protect myself
                    {
                        UseSkill(skill); //and use it
                        break;
                    }
                    else
                    {
                        continue; //if not then I know I can protect my self so we'll go to the next spell (which will be the protection skill
                    }
                }
                else if (CanBuff) //If i'm not low or can't protect my self but can Buff myself
                {
                    if (skill.GetType() == typeof(BuffSkill)) //I check if my skill can buff me 
                    {
                        UseSkill(skill); //if yes use it 
                        break;
                    }
                    else
                    {
                        continue; //if not let's go to the next spell (which will probably be the buff skill
                    }
                }
                else //if I can't d any of this the, use skill
                {
                    UseSkill(skill);
                    break;
                }
            }
        }
        yield break;
    }

    public void UseSkill(Skill skillUsed)
    {
        float totalDamage = 0;
        //IN ENEMY SKILL !!! TARGETS[0] IS THE PLAYER
        //THE PLAYER IN ENEMY SKILL IS THE ENEMY USING THE SKILL !!!

        List<Entity> allTargets = new List<Entity>
        {
            BattleSystem.Player
        };
        foreach (Entity entity in BattleSystem.Enemies) { allTargets.Add(entity); }
        foreach (var skill in BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills)
        {
            skill.PassiveBeforeAttack(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        }
        totalDamage += skillUsed.SkillBeforeUse(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        totalDamage += skillUsed.Use(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        totalDamage += skillUsed.AdditionalDamage(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);
        foreach(Skill skills in BattleSystem.Player.Skills)
        {
            skills.UseIfAttacked(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Player, BattleSystem.Turn, totalDamage);
        }
        skillUsed.SkillAfterDamage(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);

        foreach (var skill in BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills)
        {
            skill.PassiveAfterAttack(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);
        }
    }
}