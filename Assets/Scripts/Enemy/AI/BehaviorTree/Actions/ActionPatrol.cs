using UnityEngine;

public class ActionPatrol : IBehaviorNode
{
    private float _groundCheckDistance;
    private float _wallCheckDistance;
    private float _speed;
    private LayerMask _groundMask;

    private int _direction;

    private float _turnCooldown = 0.2f;
    private float _turnTimer = 0f;

    public ActionPatrol(
        float groundCheckDistance,
        float wallCheckDistance,
        float speed,
        LayerMask groundMask)
    {
        _groundCheckDistance = groundCheckDistance;
        _wallCheckDistance = wallCheckDistance;
        _speed = speed;
        _groundMask = groundMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        Transform self = context.Self;
        _turnTimer -= deltaTime;

        _direction = self.localScale.x > 0 ? 1 : -1;

        Vector2 pos = self.position;

        Vector2 forward = new Vector2(_direction, 0);
        Vector2 groundCheckPos = pos + new Vector2(_direction * 0.4f, 0);

        bool wallHit = Physics2D.Raycast(pos, forward, _wallCheckDistance, _groundMask);

        bool groundHit = Physics2D.Raycast(
            groundCheckPos,
            Vector2.down,
            _groundCheckDistance,
            _groundMask);

        if (_turnTimer <= 0 && (wallHit || !groundHit))
        {
            context.Status.Idle();
            context.Mover.Stop();

            _turnTimer = _turnCooldown;

            return NodeState.Success;
        }

        context.Status.Move();
        context.Mover.MoveSpeed = _speed;
        context.Mover.MoveDirection(forward);

        return NodeState.Running;
    }
}