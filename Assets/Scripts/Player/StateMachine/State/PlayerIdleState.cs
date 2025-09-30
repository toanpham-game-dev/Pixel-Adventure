using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerController _player;
    private PlayerStateMachine _stateMachine;

    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _player.Anim.PlayAnimation("Idle");
    }

    public void Update()
    {
        if (_player.Input.Move != 0)
        {
            _stateMachine.TransitionTo(_player.MoveState); // Transition to Move state
        }
    }

    public void FixedUpdate()
    {
        _player.Movement.Run();
    }

    public void Exit()
    {

    }
}
