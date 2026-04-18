using UnityEngine;

public class ActionMoveToTargetGround : IBehaviorNode
{
    private const float STOP_DISTANCE = 0.4f;

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Target == null || context.Mover == null)
            return NodeState.Failure;

        Vector2 selfPos = context.Self.position;
        Vector2 targetPos = context.Target.position;

        float dx = targetPos.x - selfPos.x;

        if (Mathf.Abs(dx) < STOP_DISTANCE)
        {
            context.Mover.Stop();
            return NodeState.Running;
        }

        Vector2 movePos = new Vector2(targetPos.x, selfPos.y);

        context.Mover.MoveTowards(movePos);

        return NodeState.Running;
    }
}