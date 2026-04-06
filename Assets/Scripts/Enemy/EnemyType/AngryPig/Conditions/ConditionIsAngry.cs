public class ConditionIsAngry : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State == EnemyState.Chase)
            return NodeState.Success;

        return NodeState.Failure;
    }
}