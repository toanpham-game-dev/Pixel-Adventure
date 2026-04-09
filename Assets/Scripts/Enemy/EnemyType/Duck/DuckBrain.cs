using System.Collections.Generic;
using UnityEngine;

public class DuckBrain : EnemyBrain
{
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private LayerMask _wallCheckMask;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundCheckMask;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _forwardForce;
    [SerializeField] private float _idleTime;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override IBehaviorNode CreateBehaviorTree()
    {
        return new SelectorNode(new List<IBehaviorNode>
    {
        new SequenceNode(new List<IBehaviorNode>
        {
            new ConditionIsDead(),
            new ActionDie()
        }),

        new SequenceNode(new List<IBehaviorNode>
        {
            new ActionCheckWallFlip(_wallCheckDistance, _wallCheckMask),
            new ActionJumpForward(_rb, _jumpForce, _forwardForce, _groundCheckDistance, _groundCheckMask),
            new ActionIdleTimer(_idleTime, _rb)
        })
    });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3)(dir * _wallCheckDistance)
        );

        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * _groundCheckDistance));
    }
}
