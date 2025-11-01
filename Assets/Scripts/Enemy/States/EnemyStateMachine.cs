using UnityEngine;

public enum StateType
{
    Patrol,
    Chase,
    Attack,
    Hit,
    Dead
}

public class EnemyStateMachine : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }

    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void TransitionTo(IEnemyState newState)
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
