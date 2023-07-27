using System.Collections.Generic;

public class AspectoftheDemonicAngel : PassiveSkill
{
    //TODO -> When Hp is lower than 30% of max health, activate Berserk, gives undispellable attack and Crit dmg buff. 
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        if (caster.CurrentHp < (caster.Stats[Attribute.HP].Value * 0.3f))
        {
            caster.ApplyEffect(new Berserk());
        }
    }
}   
