using UnityEngine;

public class ActionAngryPatrol : IBehaviorNode
{
    private float _groundCheckDistance;
    private float _wallCheckDistance;
    private float _speed;
    private LayerMask _groundMask;

    public ActionAngryPatrol(
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

        int direction = self.localScale.x > 0 ? 1 : -1;

        Vector2 pos = self.position;

        Vector2 forward = new Vector2(direction, 0);
        Vector2 groundCheckPos = pos + new Vector2(direction * 0.4f, 0);

        bool wallHit = Physics2D.Raycast(pos, forward, _wallCheckDistance, _groundMask);

        bool groundHit = Physics2D.Raycast(
            groundCheckPos,
            Vector2.down,
            _groundCheckDistance,
            _groundMask);

        if (wallHit || !groundHit)
        {
            Flip(context);
            return NodeState.Running;
        }

        context.Status.Chase();

        context.Mover.MoveSpeed = _speed * 2f;
        context.Mover.MoveDirection(forward);

        return NodeState.Running;
    }

    private void Flip(EnemyContext context)
    {
        Vector3 scale = context.Self.localScale;
        scale.x *= -1;
        context.Self.localScale = scale;
    }
}