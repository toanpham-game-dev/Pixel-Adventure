using UnityEngine;

public class ActionDiveAttack : IBehaviorNode
{
    private Rigidbody2D _rb;
    private float _diveSpeed;
    private float _flyUpSpeed;
    private Vector2 _originPos;
    private LayerMask _groundMask;
    private float _groundCheckDistance;

    private bool _isDiving = false;
    private bool _isFlyingUp = false;

    public ActionDiveAttack(
        Rigidbody2D rb,
        float diveSpeed,
        Vector2 originPos,
        float flyUpSpeed,
        LayerMask groundMask,
        float groundCheckDistance)
    {
        _rb = rb;
        _diveSpeed = diveSpeed;
        _originPos = originPos;
        _flyUpSpeed = flyUpSpeed;
        _groundMask = groundMask;
        _groundCheckDistance = groundCheckDistance;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        FatBirdStatus status = context.Status as FatBirdStatus;

        if (!_isDiving && !_isFlyingUp)
        {
            _isDiving = true;
            status.IsDiving = true;
            context.Status.Attack();
        }

        if (_isDiving)
        {
            _rb.linearVelocity = new Vector2(0, -_diveSpeed);

            RaycastHit2D groundCheck = Physics2D.Raycast(
                _rb.position,
                Vector2.down,
                _groundCheckDistance,
                _groundMask
            );

            if (groundCheck.collider != null)
            {
                _isDiving = false;
                _isFlyingUp = true;
                _rb.linearVelocity = Vector2.zero;
                context.Status.IsGrounded();
            }

            return NodeState.Running;
        }

        if (_isFlyingUp)
        {
            Vector2 currentPos = _rb.position;
            Vector2 targetPos = new Vector2(_originPos.x, _originPos.y);

            Vector2 newPos = Vector2.MoveTowards(
                currentPos,
                targetPos,
                _flyUpSpeed * deltaTime
            );

            _rb.MovePosition(newPos);
            context.Status.FlyUp();

            if (Vector2.Distance(newPos, targetPos) < 0.05f)
            {
                _isFlyingUp = false;
                status.IsDiving = false;
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        return NodeState.Failure;
    }
}