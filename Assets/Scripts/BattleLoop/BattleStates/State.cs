using System.Collections;
using System.Net;
using UnityEngine;

    public abstract class State {
        
        protected BattleSystem BattleSystem;

        public State(BattleSystem battleSystem)
        {
            BattleSystem = battleSystem;
        }
        public virtual IEnumerator Start()
        {
            yield break;
        }

        
        
    }


   
