using UnityEngine;

/// <summary>
/// Condition node that becomes permanently true once the bat enters Chase state.
/// - While in Idle: checks if target is within view range using OverlapCircle.
/// - Once in Chase: always returns Success (so the BT never falls back to Idle).
/// </summary>
public class ConditionCanSeeTarget : IBehaviorNode
{
    private readonly LayerMask _targetMask;

    public ConditionCanSeeTarget(LayerMask targetMask)
    {
        _targetMask = targetMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Target == null)
            return NodeState.Failure;

        var status = context.Self.GetComponent<BatStatus>();

        // If we are already in Chase state, this condition is always true.
        if (status != null && status.State == BatState.Chase)
        {
            return NodeState.Success;
        }

        // Still in Idle -> check detection by circle
        Vector2 center = context.Self.position;
        Collider2D hit = Physics2D.OverlapCircle(center, context.ViewRange, _targetMask);

        // Debug.Log($"[CanSee] center={center}, range={context.ViewRange}, hit={hit}");

        if (hit != null && hit.transform == context.Target)
        {
            // Optional: you can set Chase immediately here as well
            status?.BatChase();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
