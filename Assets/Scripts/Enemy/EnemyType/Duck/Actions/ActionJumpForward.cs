using UnityEngine;

public class ActionJumpForward : IBehaviorNode
{
    private Rigidbody2D _rb;
    private float _jumpForce;
    private float _forwardForce;

    private float _groundCheckDistance;
    private LayerMask _groundMask;

    private bool _jumped;
    private bool _leftGround;

    public ActionJumpForward(
        Rigidbody2D rb,
        float jumpForce,
        float forwardForce,
        float groundCheckDistance,
        LayerMask groundMask)
    {
        _rb = rb;
        _jumpForce = jumpForce;
        _forwardForce = forwardForce;
        _groundCheckDistance = groundCheckDistance;
        _groundMask = groundMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State == EnemyState.Idle)
            return NodeState.Success;

        if (!_jumped)
        {
            float dir = context.Self.transform.localScale.x > 0 ? 1 : -1;

            _rb.linearVelocity = new Vector2(dir * _forwardForce, _jumpForce);

            context.Status.Move();

            _jumped = true;
            _leftGround = false;

            return NodeState.Running;
        }

        bool grounded = Physics2D.Raycast(
            context.Self.transform.position,
            Vector2.down,
            _groundCheckDistance,
            _groundMask
        );

        if (!grounded)
            _leftGround = true;

        if (_leftGround && grounded)
        {
            _jumped = false;
            context.Status.Idle();
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}