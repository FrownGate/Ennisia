using Unity.VisualScripting;
using UnityEngine;


    public abstract class StateMachine : MonoBehaviour
    {
        protected State State;

        public void SetState(State state)
        {
            State = state;
            StartCoroutine(State.Start());
        }


    }
    
    
    
