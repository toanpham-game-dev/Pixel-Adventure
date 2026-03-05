using UnityEngine;

public class PigPatrolGroundNode : IBehaviorNode
{
    private readonly float _groundCheckDistance;
    private readonly float _wallCheckDistance;
    private readonly float _walkSpeed;
    private readonly float _runSpeed;
    private readonly LayerMask _groundMask;

    private int _direction = -1;

    private readonly float _baseScaleX;

    public PigPatrolGroundNode(Transform self, float groundCheckDistance, float wallCheckDistance, float walkSpeed, float runSpeed, LayerMask groundMask)
    {
        _groundCheckDistance = groundCheckDistance;
        _wallCheckDistance = wallCheckDistance;
        _walkSpeed = walkSpeed;
        _runSpeed = runSpeed;
        _groundMask = groundMask;
        _baseScaleX = Mathf.Abs(self.localScale.x);
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        var status = context.Self.GetComponent<AngryPigStatus>();
        var rb = context.Self.GetComponent<Rigidbody2D>();
        if (status == null || rb == null)
            return NodeState.Failure;

        if (status.State == AngryPigState.Dead)
            return NodeState.Failure;

        Vector2 pos = context.Self.position;
        Vector2 forward = new Vector2(_direction, 0f);

        RaycastHit2D groundHit = Physics2D.Raycast(status.transform.position, Vector2.down, _groundCheckDistance, _groundMask);

        RaycastHit2D wallHit = Physics2D.Raycast(status.transform.position, forward, _wallCheckDistance, _groundMask);

        if (!groundHit || wallHit)
        {
            _direction *= -1;
        }

        float speed = status.State == AngryPigState.Run ? _runSpeed : _walkSpeed;

        rb.linearVelocity = new Vector2(_direction * speed, rb.linearVelocity.y);

        // Flip direction
        if (_direction != 0)
        {
            var scale = context.Self.localScale;
            scale.x = -_direction * _baseScaleX;
            context.Self.localScale = scale;
        }

        return NodeState.Running;
    }
}
