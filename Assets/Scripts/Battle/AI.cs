using System.Collections.Generic;
using UnityEngine;

public abstract class AI
{
    private static Entity FindLowestEnemy(List<Entity> enemies)
    {
        int id = 0;
        Entity lowestEnemy = null;

        foreach (Entity enemy in enemies)
        {
            if (lowestEnemy == null || enemy.CurrentHp < lowestEnemy.CurrentHp)
            {
                lowestEnemy = enemy;
                id = enemies.IndexOf(enemy);
            }
        }
        return lowestEnemy;
    }

    private static List<Entity> FindBreakDefEnemy(List<Entity> ennemies)
    {
        List<Entity> bdef = new();

        foreach (Entity enemy in ennemies)
        {
            if (enemy.Effects.Find(effect => effect.GetType() == typeof(BreakDefense)) != null)
            {
                bdef.Add(enemy);
            }
        }

        return bdef;
    }

    public static Entity FindBestEnnemy(List<Entity> enemies)
    {
        if (FindBreakDefEnemy(enemies).Count > 0)
        {
            return FindLowestEnemy(FindBreakDefEnemy(enemies));
        }
        else
        {
           return FindLowestEnemy(enemies);
        }
    }

    private static bool CanProtect(Entity caster)
    {
        foreach (Skill skill in caster.Skills)
        {
            if (skill.GetType() == typeof(ProtectionSkill) && skill.Cooldown == 0)
            {
                return true;
            }
        }
        return false;
    }

    private static bool CanBuff(Entity caster)
    {

        //if(caster.Skills.Contains(BuffSkill)) ERROR
        foreach (Skill skill in caster.Skills)
        {
            if (skill.GetType() == typeof(BuffSkill) && skill.Cooldown == 0)
            {
                return true;
            }
        }
        return false;
    }

    private static bool AllyLowAndAoeCheck(int casterID, BattleSystem battlesystem, bool enemyTurn)
    {
        if (!enemyTurn) { return false; }

        bool CanHealAOE = false;

        foreach (Skill skill in battlesystem.Enemies[casterID].Skills)
        {
            if(skill.GetType() == typeof(ProtectionSkill) && skill.Data.AOE)
            {
                CanHealAOE = true;
                break;
            }
        }

        foreach (Entity enemy in battlesystem.Enemies)
        {
            if (enemy.CurrentHp < enemy.Stats[Attribute.HP].Value * 20 / 100)
            {
                if(battlesystem.Enemies.IndexOf(enemy) != casterID && CanHealAOE )
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static int ChooseSkill(BattleSystem battleSystem, Entity caster, bool enemyTurn)
    {
        List<Skill> list = caster.Skills;
        list.Reverse();
        Skill skillToUse = null;

        foreach (Skill skill in list)
        {
            if (skill.Cooldown == 0 && skill.GetType() != typeof(PassiveSkill))
            {
                if ((caster.CurrentHp < caster.Stats[Attribute.HP].Value * 20 / 100 || AllyLowAndAoeCheck(battleSystem.EnemyPlayingID, battleSystem, enemyTurn)) && CanProtect(caster))
                { //if i'm low and i can protect myself // modify to check all the team hp

                    if (skill.GetType() == typeof(ProtectionSkill)) //i check if my skill alow me to protect myself
                    {
                        skillToUse = skill; //and use it
                        break;
                    }
                    else
                    {
                        continue; //if not then I know I can protect my self so we'll go to the next spell (which will be the protection skill
                    }
                }
                else if (CanBuff(caster)) //If i'm not low or can't protect my self but can Buff myself
                {
                    if (skill.GetType() == typeof(BuffSkill)) //I check if my skill can buff me 
                    {
                        skillToUse = skill; //if yes use it 
                        break;
                    }
                    else
                    {
                        continue; //if not let's go to the next spell (which will probably be the buff skill
                    }
                }
                else //if I can't d any of this the, use skill
                {
                    skillToUse = skill;
                    break;
                }
            }
        }
        list.Reverse();

        Debug.Log($"{caster.Name} selected {skillToUse.Data.Name}");
        return list.IndexOf(skillToUse);
    }
}