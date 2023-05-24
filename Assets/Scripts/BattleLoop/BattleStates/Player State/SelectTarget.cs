using System.Collections;
using Entities;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class SelectTarget : SelectSpell
    {
        public SelectTarget(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            BattleSystem.dialogueText.text = "Select a target";

            yield return new WaitForSeconds(2.0f);
            
            
        }
        
        public override IEnumerator Attack()
        {
            //Attack Button
            if (_spellNumber == 0 )
            {
                /*foreach (var enemy in BattleSystem.Enemies)
                {
                    if(CheckEnemyIsDead(enemy)) continue;
                    if (enemy.IsSelected)
                    {
                        //enemy.TakeDamage(BattleSystem.PlayerData.damage);
                    }
                }*/
            }else if (_spellNumber == 1)//Spell Button 1 
            {
                
            }else if (_spellNumber == 2)//Spell Button 2 
            {
                
            }
            
            //Set HP bar
            //BattleSystem.enemyHUD.SetHp(BattleSystem.EnemyData.currentHp);
            
            /*if (BattleSystem.EnemyData.IsDead)
            {
                BattleSystem.SetState(new Won(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
            }*/
            
            
            yield return new WaitForSeconds(1f);
        }

        
        private bool CheckEnemyIsDead(Entity enemy)
        {
            if (enemy.IsDead)
            {
                return true;
            }
            
            return false;
        }
    }
}