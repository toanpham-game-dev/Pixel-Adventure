using UnityEngine;

/// <summary>
/// Condition node that checks if the enemy can detect the target
/// within its view range.
/// </summary>
public class ConditionCanSeeTarget : IBehaviorNode
{
    private LayerMask _targetMask;

    public ConditionCanSeeTarget(LayerMask targetMask)
    {
        _targetMask = targetMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Target == null)
            return NodeState.Failure;

        if (context.Status.State == EnemyState.Chase)
            return NodeState.Success;

        Vector2 center = context.Self.position;

        Collider2D hit = Physics2D.OverlapCircle(
            center,
            context.ViewRange,
            _targetMask
        );

        if (hit != null && hit.transform == context.Target)
        {
            context.Status.Chase();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}