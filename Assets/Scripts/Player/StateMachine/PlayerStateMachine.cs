using UnityEngine;

public class PlayerStateMachine
{
    public IPlayerState CurrentState { get; private set; }

    public void Initialize(IPlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void TransitionTo(IPlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState.Update();
    }

    public void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }
}
