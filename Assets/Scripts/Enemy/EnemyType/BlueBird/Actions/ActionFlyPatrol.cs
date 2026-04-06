using UnityEngine;

public class ActionFlyPatrol : IBehaviorNode
{
    private float _speed;

    private float _wallCheckDistance;
    private Vector2 _wallCheckSize;

    private LayerMask _wallMask;

    private float _turnCooldown = 0.2f;
    private float _turnTimer = 0f;

    private int _direction;

    public ActionFlyPatrol(
        float speed,
        float wallCheckDistance,
        Vector2 wallCheckSize,
        LayerMask wallMask)
    {
        _speed = speed;
        _wallCheckDistance = wallCheckDistance;
        _wallCheckSize = wallCheckSize;
        _wallMask = wallMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        Transform self = context.Self;

        _turnTimer -= deltaTime;

        _direction = self.localScale.x > 0 ? 1 : -1;

        Vector2 pos = self.position;
        Vector2 dir = new Vector2(_direction, 0f);

        RaycastHit2D hit = Physics2D.BoxCast(
            pos,
            _wallCheckSize,
            0f,
            dir,
            _wallCheckDistance,
            _wallMask
        );

        if (_turnTimer <= 0 && hit)
        {
            context.Status.Idle();
            context.Mover.Stop();

            _turnTimer = _turnCooldown;

            return NodeState.Success;
        }

        context.Status.Move();

        context.Mover.MoveSpeed = _speed;
        context.Mover.MoveDirection(dir);

        return NodeState.Running;
    }
}