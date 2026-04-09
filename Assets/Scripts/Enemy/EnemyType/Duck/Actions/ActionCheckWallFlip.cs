using UnityEngine;

public class ActionCheckWallFlip : IBehaviorNode
{
    private LayerMask _groundMask;
    private float _wallCheckDistance;

    public ActionCheckWallFlip(float wallCheckDistance, LayerMask groundMask)
    {
        _wallCheckDistance = wallCheckDistance;
        _groundMask = groundMask;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State == EnemyState.Move)
            return NodeState.Success;
        Vector2 dir = context.Self.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            context.Self.transform.position,
            dir,
            _wallCheckDistance,
            _groundMask);

        if (hit.collider != null)
        {
            Vector3 scale = context.Self.transform.localScale;
            scale.x *= -1;
            context.Self.transform.localScale = scale;
            Debug.Log("Hit wall");
        }

        return NodeState.Success;
    }
}