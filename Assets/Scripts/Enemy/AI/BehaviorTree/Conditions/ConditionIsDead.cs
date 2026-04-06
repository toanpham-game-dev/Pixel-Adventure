public class ConditionIsDead : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status == null)
            return NodeState.Failure;

        if (context.Status.State == EnemyState.Dead)
            return NodeState.Success;

        return NodeState.Failure;
    }
}