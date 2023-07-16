using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State;

    public void SetState(State state)
    {
        Debug.Log(state);
        State = state;
        StartCoroutine(State.Start());
    }
}