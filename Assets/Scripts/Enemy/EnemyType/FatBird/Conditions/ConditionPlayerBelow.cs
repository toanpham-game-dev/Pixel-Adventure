using UnityEngine;

public class ConditionPlayerBelow : IBehaviorNode
{
    private LayerMask _targetMask;
    private LayerMask _groundMask;
    private float _detectWidth;
    private float _detectHeight;

    public static bool IsDiving;

    public ConditionPlayerBelow(
        LayerMask targetMask,
        LayerMask groundMask,
        float detectWidth,
        float detectHeight)
    {
        _targetMask = targetMask;
        _groundMask = groundMask;
        _detectWidth = detectWidth;
        _detectHeight = detectHeight;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        FatBirdStatus status = context.Status as FatBirdStatus;

        if (status.IsDiving)
            return NodeState.Success;

        Vector2 origin = context.Self.position;

        LayerMask mask = _targetMask | _groundMask;

        RaycastHit2D groundHit = Physics2D.Raycast(
            origin,
            Vector2.down,
            Mathf.Infinity,
            _groundMask
        );

        float detectDistance = _detectHeight;

        if (groundHit.collider != null)
        {
            detectDistance = groundHit.distance;
        }

        Debug.DrawRay(origin, Vector2.down * detectDistance, Color.yellow);

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            new Vector2(_detectWidth, _detectHeight),
            0f,
            Vector2.down,
            detectDistance,
            _targetMask
        );

        if (hit.collider != null)
        {
            // Check target layer
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}