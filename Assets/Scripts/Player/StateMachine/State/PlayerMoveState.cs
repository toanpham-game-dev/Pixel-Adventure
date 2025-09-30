using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    private PlayerController _player;
    private PlayerStateMachine _stateMachine;

    public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _player.Anim.PlayAnimation("Run");
    }

    public void Update()
    {
        if (_player.Input.Move == 0)
        {
            _stateMachine.TransitionTo(_player.IdleState); // Transition to Idle state
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
