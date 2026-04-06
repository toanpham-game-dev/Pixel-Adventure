using UnityEngine;

public class ActionBatIdle : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State == EnemyState.Idle)
        {
            context.Mover?.Stop();
            return NodeState.Running;
        }

        return NodeState.Failure;
    }
}