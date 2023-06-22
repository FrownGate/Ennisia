using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading;
using static Stat<float>;

public class EarthenWard : ProtectionSkill
{
    ModifierID id;
    float shieldAmount;
//TODO -> Once every 2 turns, give a barrier scaling based off of max Hp.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    { 
        if (turn%2 == 0)
        {
            player.Shield += (int)player.Stats[Item.AttributeStat.HP].Value * (20 / 100);
        }
    }
}
