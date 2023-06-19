using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading;

public class EarthenWard : Skill
{
//TODO -> Once every 2 turns, give a barrier scaling based off of max Hp.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    { 
        if (turn%2 == 0)
        {
            //give a barrier scaling based off of max Hp.
        }
    }

}
