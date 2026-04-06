public class ConditionCanIdle : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status == null)
            return NodeState.Failure;

        if (context.Status.State == EnemyState.Idle)
            return NodeState.Success;

        return NodeState.Failure;
    }
}